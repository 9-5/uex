# uex - URL Executor for Windows

A simple, powerful command-line tool that brings Unix-like remote script execution to the Windows Command Prompt.

# What is uex?

In Linux, it's common to execute a remote script with a single command, like `curl https://example.com/script.sh | sh`. The only native support for this is within Powershell, via `irm https://example.com/script.ps1 | iex` This is not natively possible in the Windows Command Prompt (cmd.exe).

uex (URL Execute) bridges this gap. It's a small, portable utility that allows you to securely execute remote Windows batch scripts (.bat) directly from a URL or a pipe, without manually downloading the file first.

## Features

- Direct URL Execution: Run a script directly from its URL.

- Piped Execution: Works seamlessly with curl or other tools that output to stdout.

- Security-Focused: Enforces the use of https:// to prevent insecure downloads over HTTP.

- Portable: A single, dependency-less executable. No installation required.

- Multi-Architecture: Pre-compiled for x64, x86, and Arm64 Windows systems.

## Usage

uex supports two primary modes of operation.
1. Direct URL Execution

Provide the HTTPS URL to the script as the only argument.

`uex https://example.com/script.bat`

2. Piped Execution

Pipe the output of curl (or another command) directly into uex.

`curl https://example.com/script.bat | uex`


## Installation

1. Navigate to the [Releases](https://github.com/9-5/uex/releases) page.

2. Choose the executable that matches your system's architecture:

- `uex-x64.exe` for 64-bit Windows (most common).

- `uex-x86.exe` for 32-bit Windows.

- `uex-arm64.exe` for Windows on Arm devices.

3. Place the executable in a folder that is part of your system's PATH environment variable `(e.g., C:\Windows\System32)` to make it accessible from anywhere in the command line.

## Testing Your uex Setup

To ensure uex is working correctly, you can use the following test script. It verifies that variables, goto jumps, loops, and user input are all handled properly.
Test Script (test-script.bat)
```
@echo off
:: =====================================
:: :: Test Script for the 'uex' utility
:: =====================================
REM This is another style of comment.

echo.
echo --- UEX Test Script Initialized ---
echo This test will now check several batch features.
echo.

:: 1. Test basic ECHO and reading environment variables
echo Hello, %USERNAME%! You're running this on computer: %COMPUTERNAME%.
echo The current date is: %DATE%
echo.

:: 2. Test setting and reading a local variable
set "MY_VAR=Variable test was successful!"
echo Testing local variables...
echo    Result: %MY_VAR%
echo.

:: 3. Test GOTO and labels to ensure script flow is handled
echo Testing GOTO jump...
goto SkipSection

echo THIS LINE SHOULD NEVER APPEAR IN THE OUTPUT.

:SkipSection
echo    GOTO jump was successful!
echo.

:: 4. Test a simple FOR loop
echo Testing a FOR loop to count from 1 to 3:
for /L %%i in (1,1,3) do (
    echo    - Loop iteration %%i
)
echo.

:: 5. Test user input
echo Testing user input...
set /p "USER_INPUT=Please type something and press Enter: "
echo You typed: "%USER_INPUT%"
echo.

echo --- Test Script Finished Successfully ---
echo.
pause
```

### How to Run the Test

1. Host the script: Copy the code above and create a new, public GitHub Gist.

2. Get the Raw URL: On the Gist page, click the "Raw" button to get the direct URL to the file.

3. Execute with uex: Run the following command in your terminal, replacing the URL with your own.

`uex https://gist.githubusercontent.com/YourUser/abc.../raw/.../test-script.bat`

If the script runs through all the steps and displays the final success message, your `uex` tool is working perfectly.

## Security Warning

Executing scripts from the internet is inherently dangerous. You are giving code from a remote source the ability to run commands on your computer.

- Only run scripts from sources you absolutely trust.

- Review the script's code before running it to understand what it does.

- `uex` enforces `https` to protect against man-in-the-middle attacks, but it cannot protect you from a malicious script.

Use this tool responsibly.
## Building from Source

If you wish to build uex yourself:

1) Prerequisites: Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2) Clone the repository: `git clone https://github.com/9-5/uex.git`

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.