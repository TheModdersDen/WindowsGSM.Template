<!--
 Copyright (c) 2024 Bryan Hunter (TheModdersDen) | https://github.com/TheModdersDen

 Permission is hereby granted, free of charge, to any person obtaining a copy of
 this software and associated documentation files (the "Software"), to deal in
 the Software without restriction, including without limitation the rights to
 use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 the Software, and to permit persons to whom the Software is furnished to do so,
 subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 -->

# The WindowsGSM.Palworld Plugin

## By TheModdersDen

### This plugin is for use with [WindowsGSM](https://windowsgsm.com/), a powerful tool to create, manage and monitor game servers on Windows

## Requirements

- [WindowsGSM](https://windowsgsm.com/)
- [A Steam Account](https://store.steampowered.com/join/)
- [Palworld](https://store.steampowered.com/app/1623730/Palworld/)
- [Access to the Palworld Dedicated Server in your Steam Library (Under the `Tools` Category)](https://steamdb.info/app/2394010/)
- [A Palworld Dedicated Server Token](https://steamcommunity.com/dev/managegameservers)

## Installation

### Manual Installation

To manually install the plugin, follow these steps:

1. Download the [latest release](https://github.com/TheModdersDen/WindowsGSM.Palworld/releases/latest) of the plugin.
2. Extract the `WindowsGSM.Palworld-XXX.zip` file into your `plugins` directory (the `XXX` being the version identifier).
3. [Follow the instructions below to enable the plugin](#enable-the-plugin).
4. [Follow the instructions below to configure the plugin](#configuration).
5. And finally, [Follow the instructions below to install and configure your Palworld Dedicated Server](#install-the-palworld-dedicated-server).

### Enable the Plugin

To enable the plugin, follow these steps:

1. Open WindowsGSM.
2. Click on `Plugins` menu on the left-hand side (the puzzle icon).
3. Click on `Import Plugin` at the top.
4. Click on `Browse` and select the `WindowsGSM.Palworld-XXX.zip` file (the `XXX` being the version identifier).
5. Click on `Install` and then click `Close` once the plugin has been installed.

### Configuration

If needed, some manual configuration can be done to the plugin. To do so, follow these steps (in your preferred text editor-I recommend Visual Studio Code):

1. Open the directory in which you installed the plugin (found under your `WindowsGSM` directory, under `plugins`).
2. Right click on the `WindowsGSM.Palworld.cs` directory and click on `Open with Code` (or, if using a different text editor, click `Open In...`, followed by the nam e of your text editor).
3. And edit the `WindowsGSM.Palworld.cs` file to your liking
   1. specifically the values in the section beginning with `// ==================== USER CONFIGURABLE VALUES ==================== //`.
4. Save the file and close your text editor.
5. Restart WindowsGSM.
6. And finally, [Follow the instructions below to install and configure your Palworld Dedicated Server](#install-the-palworld-dedicated-server).

### Install the Palworld Dedicated Server

To install the Palworld Dedicated Server, follow these steps:

1. Open WindowsGSM.
2. Click on `Servers` menu on the left-hand side (the server icon).
3. Click on `+ Add` at the top.
4. Select `Palworld Dedicated Server` from the dropdown list.
5. Wait for the installer to download and install the Palworld Dedicated Server.
6. Once the installation is complete, click on `Start` to start the server.
7. And finally, [Follow the instructions below to configure your Palworld Dedicated Server](#configure-the-palworld-dedicated-server).

### Configure the Palworld Dedicated Server

To configure the Palworld Dedicated Server, follow these steps:

1. Open WindowsGSM.
2. Click on `Servers` menu on the left-hand side (the server icon).
3. Click on the server you want to configure.
4. Click on `Stop` to stop the server.
5. Note the `ID` for the server (you will need this later).
6. Go to the folder in which you installed the server (found under your `WindowsGSM` directory, under `serverfiles`).
7. In this folder, go to the following directory (where `ID` is the `ID` you noted earlier): `WindowsGSM_Installation\servers\ID\PalServer\Pal\Saved\Config\WindowsServer\`.
8. Edit the `PalWorldSettings.ini` file to your liking. Make sure to keep the file in the `WindowsServer` directory.
   1. Also, make sure to keep the file limited to two lines:
   2. The first line should be `[/Script/Pal.PalWorldSettings]`.
   3. The second line is (unfortunately) the unholy entirety of the configuration file.
9. Save the file and close your text editor.
10. Go to WindowsGSM and click on `Start` to start the server.
11. And finally, [Follow the instructions below to connect to your Palworld Dedicated Server](#connect-to-the-palworld-dedicated-server).

### Connect to the Palworld Dedicated Server

To connect to the Palworld Dedicated Server, follow these steps:

1. Open Palworld.
2. Click on `Play` at the top.
3. Click on `Join Game` at the top.
4. Click on `Direct Connect` at the top.
5. Enter the IP address of your server (found in the `WindowsGSM` console).
   1. It should be on a line that looks like this: `Current public ip address: 'WWW.XXX.YYY.ZZZ'`.
6. Click on `Connect` at the bottom.
7. And finally, enjoy your Palworld Dedicated Server!

## Support

If you have any questions, please contact me at <https://themoddersden.com/contact/>. I will try to respond as soon as possible.

## Contributing

If you would like to contribute to this project, please read the [CONTRIBUTING.md](docs/CONTRIBUTING.md) file.

## License

This project is licensed under a custom derivative of the [MIT License](docs/LICENSE.md). Please read the [LICENSE.md](docs/LICENSE.md) file for more information.

## Credits

- [TheModdersDen](https://github.com/TheModdersDen) - Project Creator
- [WindowsGSM](https://windowsgsm.com/) - WindowsGSM
- [Palworld](https://store.steampowered.com/app/1623730/Palworld/) - Palworld
- [GitHub](https://github.com/) - Code Hosting
- [GitHub CoPilot](https://copilot.github.com/) - Code Suggestions/Auto-Completion
- [Visual Studio Code](https://code.visualstudio.com/) - Code Editor
- [CircleCI](https://circleci.com/) - Continuous Integration

## Changelog

For a list of changes, please see the [CHANGELOG.md](docs/CHANGELOG.md) file.