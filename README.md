# htmltool

Description:
  HTML tool

Usage:
  htmltool [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  screenshot  Render HTML into PNG


# htmltool screenshot

Description:
  Render HTML into PNG

Usage:
  htmltool screenshot [options]

Options:
  --interactive                                                       Interactive mode
  -i, --input <input> (REQUIRED)                                      Input HTML url
  -o, --output <output>                                               Output PNG filename [default: out.png]
  --cache-path <cache-path>                                           Cache path [default: htmltool_cache]
  --width <width>                                                     Browser window width [default: 1366]
  --height <height>                                                   Browser window height [default: 768]
  --dpi <dpi>                                                         Browser window DPI [default: 192]
  --delay <delay>                                                     Delay, milliseconds [default: 1000]
  --cef-log-file <cef-log-file>                                       CEF log file
  --cef-log-level <Default|Disable|Error|Fatal|Info|Verbose|Warning>  CEF log level [default: Default]
  -?, -h, --help                                                      Show help and usage information
