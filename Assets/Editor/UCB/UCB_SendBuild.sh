#!/bin/sh

# Unity Cloud Build: Auto push build result to github (Post-build script)
# Original: https://stackoverflow.com/questions/60791806/unity-cloud-build-post-build-script-upload-to-github-pages
# Modded by JCxYIS 20220510
# Enviroment Vaiables to set in UCB config
#  - GITHUB_NAME = your github account name
#  - GITHUB_MAIL = your github account email
#  - GITHUB_REPO = the repo name you wanna push to
#  - GITHUB_PSW  = your github personal access token (u can get it from github > settings > developer settings > token)

set -x

export buildfolder=$1
export buildNumber=$2
export cpOptions=$3

# Not found?
if [ -z "$buildfolder" ]; then
    echo "Could not find build folder"
    exit 1
fi

# Clone the target repository to "targetRepo"
if [ ! -d ./targetRepo ]; then
    echo "Target repo not cloned. Now cloning..."
    git clone "https://${GITHUB_PSW}@github.com/${GITHUB_NAME}/${GITHUB_REPO}" ./targetRepo
fi

# sync the repo
echo "Sync repo..."
cd targetRepo
git reset --hard
git pull

# copy build folder to target folder
echo "Copy build folder to target folder..."
cd ..
cp $cpOptions "$buildfolder/" targetRepo/ # Note: macOS has no -T option
echo "$2" > targetRepo/buildNumber.txt

# git add and push
echo "Git add and push..."
cd targetRepo
git config --global user.email $GITHUB_MAIL
git config --global user.name "UCB AutoPush"
git add -A
git commit -m "Auto pushed by UCB AutoPush (Build #$buildNumber)"
git push #--force

echo "Auto push build result to github: success"