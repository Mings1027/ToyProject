# UnityGame
2023-1-9 unity test
git add .
git commit -m " "
git push

—————————————homebrew설치—————————————-
1. /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
2. Next step나오면 echo로 시작하는 명령어 복붙 엔터
3. eval로 시작하는 명령어 복붙 엔터
4. brew -v 로 설치 되었는지 확인
5. 안되면 인터넷 검색

----------------------zsh Terminal setting-------------------------
Oh-my-zsh 설치
sh -c "$(curl -fsSL https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)"

brew install 은 이미 Brewfile로 했을것이므로 git clone 세개만 해준다.

1. syntax-highlighting 
> brew install zsh-syntax-highlighting

> git clone https://github.com/zsh-users/zsh-syntax-highlighting.git ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-syntax-highlighting

2. zsh-autosuggestions
> brew install zsh-autosuggestions

> git clone https://github.com/zsh-users/zsh-autosuggestions ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-autosuggestions

3. zsh-autojump
> brew install autojump

> git clone git://github.com/wting/autojump.git

open ~/.zshrc 로 파일 열어서 밑에 theme과 plugins 설정해준다.

ZSH_THEME="apple"
plugins=(git
zsh-autosuggestions
zsh-syntax-highlighting
autojump)

—————————————————ruby를 따로 설치해야하는 경우

brew install rbenv ruby-build

rbenv install -l  

rbenv install {원하는버전}

rbenv global {위에 입력한 버전} 

rbenv rehash 

gem install jekyll bundler     

bundler install   -> Gemfile있는곳에서 해야함

bundle exec jekyll serve -> 로컬서버 여는법

——————————————————————Brewfile—————————————————

tap "homebrew/bundle"
tap "homebrew/core"
tap "homebrew/cask"

brew "cask"
brew "git"
brew "smartmontools"
brew "zsh-autosuggestions"
brew "zsh-syntax-highlighting"
brew "autojump"

cask "alfred"
cask "appcleaner"
cask "blackhole-16ch"
cask "discord"
cask "drop-to-gif"
cask "firefox"
cask "iina"
cask "keka"
cask "parsec"
cask "rectangle"
cask "rider"
cask "surfshark"
cask "unity-hub"
cask "visual-studio-code"


—————————————————————-vscode update———————————————————

Unity 모듈 설치 후 
유니티에서 vs패키지 최신 업데이트 하고
Preferense -> External Editor 에서 Regenerate Project 하고 재시작

아래 오류는 이제 안 생기는거라고 함
—————————————————————-vscode Error———————————————————

OmniSharp.MSBuild.ProjectLoader
에러나면 C# Extensions 버전 낮추기
json파일에
"omnisharp.useGlobalMono": "always", 추가
—————————————————————vscode Extensions—————————————————
C#
JetBrains Rider Dark Theme
Material Icon Theme
TabOut
Unity Code Snippets
Unity Tools
——————————————————————vscode json setting—————————————————
{
    "security.workspace.trust.untrustedFiles": "open",
    "editor.codeLens": false,
    "workbench.colorCustomizations": {
        "editor.background": "#292929",
        "activityBar.background": "#292929",
        "sideBar.background": "#292929",
    },
    "editor.formatOnSave": true,
    "editor.formatOnPaste": true,
    "editor.formatOnType": true,
    "omnisharp.useGlobalMono": "always",
    "extensions.autoUpdate": false,
    "extensions.autoCheckUpdates": false,
    "editor.fontSize": 17,
    "editor.fontFamily": "Jetbrains Mono",
    "editor.fontLigatures": true,
    "editor.matchBrackets": "never",
    "editor.language.brackets": [],
    "workbench.iconTheme": "material-icon-theme",
    "workbench.colorTheme": "JetBrains Rider Dark Theme",
    "workbench.startupEditor": "none",
    "json.schemas": [],
    "window.commandCenter": true,
    "editor.tokenColorCustomizations": {
        "textMateRules": [
            {
                "scope": [
                    "entity.name.variable.field.cs",
                    "entity.name.variable.local.cs",
                    "variable.parameter",
                    "keyword.operator"
                ],
                "settings": {
                    "foreground": "#bcbcbc"
                },
            },
            {
                "scope": "variable.other.property",
                "settings": {
                    "foreground": "#6fcdd9",
                }
            }
        ]
    },
}




————————————————Unity settings—————————————————
Mono랑 .Net은 Rider 받으면 그 안에서 받을 수 있는거 같다 굳이 아래에서 안해도 될듯

Mono
https://www.mono-project.com/download/stable/#download-mac

.Net
https://dotnet.microsoft.com/en-us/download


——————————---—-————Xcode settings—————————————————
product -> scheme -> Edit scheme -> Option -> working directory 를 현재 프로젝트 위치로 설정


—————————————————Unity Sort Layer————————————————
How to sort layer : 
1. Sprite renderer -> sprite sort point -> pivot
2. Change pivot in Sprite Editor

