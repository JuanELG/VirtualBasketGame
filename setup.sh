#!/usr/bin/env bash
set -euo pipefail

echo "==> Installing essentials..."
apt-get update
apt-get install -y wget curl unzip git-lfs libgtk-3-dev libgl1-mesa-dev

echo "==> Installing Git LFS..."
git lfs install
git lfs pull || echo "No LFS files or failed pull"

echo "==> Installing Unity Hub..."
sudo sh -c 'echo "deb https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub.list'
wget -qO - https://hub.unityd3d.com/linux/keys/public | sudo apt-key add -
apt-get update
sudo apt-get install -y unityhub

echo "==> Installing Unity Editor and Android modules via Hub CLI..."
unityhub -- --headless install \
  --version "$UNITY_VERSION" \
  --module linux-il2cpp \
  --module android \
  --module android-sdk-ndk-tools \
  --module android-open-jdk

echo "==> Exporting env variables..."
if [ -f .env ]; then
  export $(grep -v '^#' .env | xargs)
else
  echo ".env not found!"
  exit 1
fi

echo "==> Exporting specific variables to environment..."
export OPENAI_API_KEY
export UNITY_VERSION
export ANDROID_SDK_PATH ANDROID_NDK_PATH
export TEST_PLATFORM TEST_RESULTS_DIR
export CODEX_MODEL CODEX_FULL_AUTO

if ! command -v unity &> /dev/null; then
  echo "Unity CLI not found, ensure installation is correct."
  exit 1
fi

UNITY_PATH="/opt/unity-editor/Unity"

echo "==> Verifying project loads in batch mode..."
"$UNITY_PATH" -batchmode -quit -projectPath "$(pwd)" -logFile /dev/null

echo "==> Installing .NET SDK..."
apt-get install -y dotnet-sdk-8.0

echo "==> Restoring .NET dependencies..."
dotnet restore || echo "No .NET dependencies found"

mkdir -p test-results

echo "==> Running EditMode tests..."
"$UNITY_PATH" -batchmode -projectPath "$(pwd)" \
  -runTests -testPlatform EditMode \
  -testResults test-results/editmode-results.xml \
  -logFile test-results/editmode-log.txt -quit

echo "==> Running PlayMode tests..."
"$UNITY_PATH" -batchmode -projectPath "$(pwd)" \
  -runTests -testPlatform PlayMode \
  -testResults test-results/playmode-results.xml \
  -logFile test-results/playmode-log.txt -quit

echo "==> Environment setup complete with Android support."
