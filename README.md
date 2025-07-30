# Music Manager

A utility to help manager and organize music files.

## Features

Fixes MP3 tags:

1. Sets blank tags (artist, album, track, etc.)
2. Sets the album artist
3. Fixes track and disc numbers
4. Etc.

## Usage

```shell
MusicManager [DIR]
```

### Argument

- **DIR**: The music directory to process.

### Options

- `-v`, `--version`
  - Display the application version.
- `-h`, `--help`
  - Display the help.

## Install

1. Install the [.NET 9.0 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).
2. Download the latest [release](https://github.com/jessekingf/music-manager/releases).

### Directory Structure

```text
- Artist 1/
  - (year) Album 1/
    - 01 Track 1.mp3
    - 02 Track 2.mp3
  - (year) Album 2/
  - ...
- Artist 2/
  - (year) Album 1/
  - Disc 1/
    - 01 Track 1.mp3
    - 02 Track 2.mp3
  - Disc 2/
    - 01 Track 1.mp3
- ...
```
