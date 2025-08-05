#!/usr/bin/env bash
set -euxo pipefail

echo "==> Installing libssl1.1 (required for Unity Hub)…"
wget -qO libssl1.1.deb http://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.0g-2ubuntu4_amd64.deb
sudo dpkg -i libssl1.1.deb

echo "==> Adding Unity Hub public key and repository…"
wget -qO - https://hub.unity3d.com/linux/keys/public \
  | gpg --dearmor \
  | sudo tee /usr/share/keyrings/Unity_Technologies_ApS.gpg > /dev/null

sudo sh -c 'echo "deb [signed-by=/usr/share/keyrings/Unity_Technologies_ApS.gpg] https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub.list'

echo "==> Installing Unity Hub…"
sudo apt-get update
sudo apt-get install -y unityhub

echo "==> Installing Unity Editor via Hub CLI…"
unityhub -- --headless install --version "$UNITY_VERSION"

echo "==> Installing Android modules (SDK, NDK, OpenJDK)…"
unityhub -- --headless install-modules \
  --version "$UNITY_VERSION" \
  -m android \
  -m android-sdk-ndk-tools \
  -m android-open-jdk

echo "==> Exporting .env variables…"
if [ -f .env ]; then
  export $(grep -v '^#' .env | xargs)
else
  echo ".env not found!" >&2
  exit 1
fi

export OPENAI_API_KEY
export UNITY_VERSION
export ANDROID_SDK_PATH ANDROID_NDK_PATH
export TEST_PLATFORM TEST_RESULTS_DIR
export CODEX_MODEL CODEX_FULL_AUTO

if ! command -v unity &> /dev/null; then
  echo "Unity CLI not found, installation failed." >&2
  exit 1
fi

UNITY_PATH="/opt/unity-editor/Unity"

echo "==> Testing Unity project loads in batch mode…"
"$UNITY_PATH" -batchmode -quit -projectPath "$(pwd)" -logFile /dev/null

echo "==> Installing .NET SDK 8.0…"
sudo apt-get install -y dotnet-sdk-8.0

echo "==> Restoring .NET dependencies…"
dotnet restore || echo "No .NET dependencies found"

mkdir -p test-results

echo "==> Running EditMode tests…"
"$UNITY_PATH" -batchmode -projectPath "$(pwd)" \
  -runTests -testPlatform EditMode \
  -testResults test-results/editmode-results.xml \
  -logFile test-results/editmode-log.txt -quit

echo "==> Running PlayMode tests…"
"$UNITY_PATH" -batchmode -projectPath "$(pwd)" \
  -runTests -testPlatform PlayMode \
  -testResults test-results/playmode-results.xml \
  -logFile test-results/playmode-log.txt -quit

echo "==> Setup complete with Unity + Android support."