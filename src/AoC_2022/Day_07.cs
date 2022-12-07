namespace AoC_2022;

public class Day_07 : BaseDay
{
    public abstract record BaseCommand();

    public record Cd(string Target) : BaseCommand()
    {
        public string Go(string path, File? file)
        {
            switch (Target)
            {
                case ".":
                    break;
                case "..":
                    path = path[..(path.LastIndexOf('/'))];
                    if (file is not null)
                    {
                        file.ParentPath = path;
                    }
                    break;
                case "/":
                    path = "/";
                    break;
                default:
                    if (Target.StartsWith(".."))
                    {
                        throw new SolvingException();
                    }
                    path = path.TrimEnd('/');
                    path += "/" + Target;
                    break;
            }

            return path;
        }
    }

    public record Ls() : BaseCommand()
    {
        public List<FileDescriptor> Output { get; } = new();
    }

    public record FileDescriptor(string Name, bool IsDirectory, long Size = -1);

    public class File
    {
        public string? ParentPath { get; set; }

        public string Name { get; set; }

        public long Size { get; set; } = -1;

        public bool IsDirectory { get; set; }

        public List<File> Files { get; set; } = new();

        public File(string name)
        {
            Name = name;
        }

        public long GetSize()
        {
            return Size == -1
                ? Size = Files.Sum(f => f.GetSize())
                : Size;
        }
    }

    private readonly List<BaseCommand> _input;

    public Day_07()
    {
        _input = ParseInput().ToList();
    }

    private static long Aggregate(File currentFile)
    {
        var result = currentFile.IsDirectory && currentFile.GetSize() <= 100_000
            ? currentFile.GetSize()
            : 0;

        return result + currentFile.Files.Sum(Aggregate);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<string, File> filePaths = new()
        {
            ["/"] = new File("/")
        };

        string pwd = string.Empty;
        File? currentFile = null;

        foreach (var command in _input)
        {
            if (command is Ls ls)
            {
                foreach (var fileDescriptor in ls.Output)
                {
                    var filePath = $"{pwd.TrimEnd('/')}/{fileDescriptor.Name}";

                    if (!filePaths.TryGetValue(filePath, out var existingFile))
                    {
                        existingFile = filePaths[filePath] = new File(filePath) { IsDirectory = fileDescriptor.IsDirectory, Size = fileDescriptor.Size, ParentPath = pwd };
                    }
                    existingFile.ParentPath = pwd;

                    currentFile!.Files.Add(existingFile);
                }
            }
            else if (command is Cd cd)
            {
                pwd = cd.Go(pwd, currentFile);
                if (filePaths.TryGetValue(pwd, out var existingFile))
                {
                    currentFile = existingFile;
                }
                else
                {
                    currentFile = filePaths[pwd] = new File(pwd);
                }
            }
        }

        var root = filePaths["/"];

        return new($"{Aggregate(root)}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
    }

    private IEnumerable<BaseCommand> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);

        while (!file.Empty)
        {
            var line = file.NextLine();

            var dollar = line.NextElement<string>(); // $
            if (dollar != "$")
            {
                throw new SolvingException();
            }

            var commandType = line.NextElement<string>();
            yield return commandType switch
            {
                "ls" => ParseLs(file),
                "cd" => ParseCd(line),
                _ => throw new()
            };
        }

        static BaseCommand ParseCd(IParsedLine line)
        {
            return new Cd(line.NextElement<string>());
        }

        static BaseCommand ParseLs(IParsedFile file)
        {
            var ls = new Ls();

            while (!file.Empty && file.PeekNextLine().PeekNextElement<string>() != "$")
            {
                var line = file.NextLine();
                var firstElement = line.NextElement<string>();
                if (firstElement == "dir")
                {
                    ls.Output.Add(new(line.NextElement<string>(), true));
                }
                else
                {
                    ls.Output.Add(new(line.NextElement<string>(), false, long.Parse(firstElement)));
                }
            }

            return ls;
        }
    }
}
