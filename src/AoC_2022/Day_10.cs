﻿using System.Text;

namespace AoC_2022;

public class Day_10 : BaseDay
{
    public abstract record Instruction
    {
        public abstract int Length();

        public abstract int Execute(int xRegister);
    }

    public record Addx(int Value) : Instruction
    {
        public const string StringRepresentation = "addx";

        public override int Length() => 2;

        public override int Execute(int xRegister) => xRegister + Value;
    }

    public record Noop : Instruction
    {
        public const string StringRepresentation = "noop";

        public override int Length() => 1;

        public override int Execute(int xRegister) => xRegister;
    }

    private readonly List<Instruction> _input;

    public Day_10()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        int instructionIndex = -1;
        int currentInstructionTime = 0;
        int xRegister = 1;

        for (int cycle = 1; cycle <= 220; ++cycle)
        {
            if (currentInstructionTime == 0)
            {
                instructionIndex++;
                currentInstructionTime = _input[instructionIndex].Length();
            }

            if (cycle == 20 || (cycle - 20) % 40 == 0)
            {
                result += (cycle * xRegister);
            }

            --currentInstructionTime;

            if (currentInstructionTime == 0)
            {
                xRegister = _input[instructionIndex].Execute(xRegister);
            }
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var sb = new StringBuilder(240);
        int result = 0;

        int instructionIndex = -1;
        int currentInstructionTime = 0;
        int xRegister = 1;

        for (int cycle = 1; cycle <= 240; ++cycle)
        {
            if (currentInstructionTime == 0)
            {
                instructionIndex++;
                currentInstructionTime = _input[instructionIndex].Length();
            }

            if (cycle == 20 || (cycle - 20) % 40 == 0)
            {
                result += (cycle * xRegister);
            }
            var crt = (cycle % 40) - 1;
            sb.Append(xRegister - 1 == crt || xRegister == crt || xRegister + 1 == crt ? '#' : '.');

            --currentInstructionTime;

            if (currentInstructionTime == 0)
            {
                xRegister = _input[instructionIndex].Execute(xRegister);
            }
        }

        var solution = Environment.NewLine + string.Join(Environment.NewLine, sb.ToString().Chunk(40).Select(ch => new string(ch)));

        return new(solution);
    }

    private IEnumerable<Instruction> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);
        while (!file.Empty)
        {
            var line = file.NextLine();
            yield return (line.NextElement<string>() == Addx.StringRepresentation ? new Addx(line.NextElement<int>()) : new Noop());
        }
    }
}
