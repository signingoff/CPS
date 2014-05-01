using System;

namespace WindowsFormsApplication1
{
    public interface IParser
    {
        Result Parser(string target);
    }

    public class Zero : IParser
    {
        public Result Parser(string target)
        {dfdf
            return Result.Success(string.Empty, target);
        }
    }

    public class Item : IParser
    {
        public Result Parser(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return Result.Fail();
            }
            else
            {
                return Result.Success(target.Substring(0, 1), target.Substring(1));
            }
        }
    }

    public class SAT : IParser
    {
        private readonly Func<string, bool> _predicate;
        private readonly IParser _parser;

        public SAT(Func<string, bool> predicate, IParser parser)
        {
            this._predicate = predicate;
            this._parser = parser;
        }

        public Result Parser(string target)
        {
            var result = this._parser.Parser(target);
            if (result.Succeeded && this._predicate.Invoke(result.Recognized))
            {
                return result;
            }

            return Result.Fail();
        }
    }

    public class IsDigitSAT : SAT
    {
        public IsDigitSAT(IParser parser)
            : base(GetIsDigitPredicate(), parser)
        {
        }

        private static Func<string, bool> GetIsDigitPredicate()
        {
            return (s) => { foreach (var c in s) { if (!char.IsDigit(c)) return false; } return true; };
        }
    }

    public class IsAlphaSAT : SAT
    {
        public IsAlphaSAT(IParser parser)
            : base(GetPredicate(), parser)
        {
        }

        private static Func<string, bool> GetPredicate()
        {
            return (s) =>
            {
                foreach (var c in s)
                {
                    if (!char.IsLetter(c)) return false;
                } return true;
            };
        }
    }

    public class OR : IParser
    {
        private readonly IParser _left;
        private readonly IParser _right;

        public OR(IParser left, IParser right)
        {
            this._left = left;
            this._right = right;
        }

        public Result Parser(string target)
        {
            var leftResult = this._left.Parser(target);
            return leftResult.Succeeded ? leftResult : this._right.Parser(target);
        }
    }

    public class SEQ : IParser
    {
        protected readonly IParser _left;
        protected readonly IParser _right;

        public SEQ(IParser left, IParser right)
        {
            this._left = left;
            this._right = right;
        }

        public virtual Result Parser(string target)
        {
            var leftResult = this._left.Parser(target);
            if (leftResult.Succeeded)
            {
                var rightResult = this._right.Parser(leftResult.Remaining);
                if (rightResult.Succeeded)
                    return Result.Contact(leftResult, rightResult);
                else
                    return Result.Fail();
            }
            else
            {
                return Result.Fail();
            }
        }
    }

    public class OneOrMany : IParser
    {
        private readonly IParser _parser;
        private readonly int _count;

        public OneOrMany(IParser parser, int count)
        {
            this._parser = parser;
            this._count = count;
        }

        public Result Parser(string target)
        {
            Result previousResult = this._parser.Parser(target);
            if (previousResult.Succeeded)
            {
                for (int i = 0; i < this._count - 1; i++)
                {
                    Result currentResult = this._parser.Parser(previousResult.Remaining);
                    if (currentResult.Succeeded)
                    {
                        previousResult = Result.Contact(previousResult, currentResult);
                    }
                    else
                    {
                        return previousResult;
                    }
                }
            }
            else
            {
                return Result.Fail();
            }

            return previousResult;
        }
    }
}
