namespace AoC_2022;

public class Day_07 : BaseDay
{
    public abstract record BaseCommand();

    public sealed record Cd(string Target) : BaseCommand()
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

    public sealed record Ls() : BaseCommand() { public List<FileDescriptor> Output { get; } = new(); }

    public sealed record FileDescriptor(string Name, bool IsDirectory, int Size = -1);

    public sealed class File
    {
        public string? ParentPath { get; set; }

        public string Name { get; init; }

        private int _size = -1;

        public int Size
        {
            init { _size = value; }
        }

        public bool IsDirectory { get; init; }

        public List<File> Files { get; } = new();

        public File(string name)
        {
            Name = name;
        }

        public int GetSize()
        {
            return _size == -1
                ? _size = Files.Sum(f => f.GetSize())
                : _size;
        }
    }

    private readonly File _root;

    public Day_07()
    {
        var input = ParseInput();
        _root = ExtractRoot(input);
    }

    public override ValueTask<string> Solve_1()
    {
        static int SumSmallerThanThreshold(File currentFile, int threshold)
        {
            var result = currentFile.IsDirectory && currentFile.GetSize() <= threshold
                ? currentFile.GetSize()
                : 0;

            return result + currentFile.Files.Sum(f => SumSmallerThanThreshold(f, threshold));
        }

        return new($"{SumSmallerThanThreshold(_root, 100_000)}");
    }

    public override ValueTask<string> Solve_2()
    {
        static void GetDirSizes(File currentFile, HashSet<int> set, int minSize)
        {
            if (currentFile.IsDirectory)
            {
                var size = currentFile.GetSize();
                if (size >= minSize)
                {
                    set.Add(size);
                    currentFile.Files.ForEach(file => GetDirSizes(file, set, minSize));
                }
            }
        }

        var minReductionNeeded = _root.GetSize() - 70000000 + 30000000;

        var set = new HashSet<int>();
        GetDirSizes(_root, set, minReductionNeeded);

        return new($"{set.Min()}");
    }

    private static File ExtractRoot(IEnumerable<BaseCommand> input)
    {
        Dictionary<string, File> filePaths = new()
        {
            ["/"] = new File("/") { IsDirectory = true }
        };

        string pwd = string.Empty;
        File? currentFile = null;

        foreach (var command in input)
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

        return filePaths["/"];
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
                    ls.Output.Add(new(line.NextElement<string>(), false, int.Parse(firstElement)));
                }
            }

            return ls;
        }
    }
}
