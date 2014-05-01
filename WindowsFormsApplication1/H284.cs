
namespace WindowsFormsApplication1
{
    //NAME = ALPHA *63(ALPHA / DIGIT / "_" )
    //ALPHA = %x41-5A / %x61-7A   ; A-Z, a-z
    //DIGIT = %x30-39             ; digits 0 through 9
    public class H284Alpha : IsAlphaSAT
    {
        public H284Alpha()
            : base(new Item())
        {
        }
    }

    public class H284Digit : IsDigitSAT
    {
        public H284Digit()
            : base(new Item())
        {
        }
    }

    public class H284Unline : IParser
    {
        public Result Parser(string target)
        {
            if (target.StartsWith("_"))
                return Result.Success("_", target.TrimStart('_'));
            else
                return Result.Fail();
        }
    }

    public class DigitOrAlphaOrUnderline : OR
    {
        public DigitOrAlphaOrUnderline()
            : base(new OR(new H284Alpha(), new H284Digit()), new H284Unline())
        {
        }
    }

    public class ZeroTo63 : OR
    {
        public ZeroTo63()
            : base(new OneOrMany(new DigitOrAlphaOrUnderline(), 64), new Zero())
        {
        }
    }

    public class H284Name : SEQ
    {
        public H284Name()
            : base(new H284Alpha(), new ZeroTo63())
        {
        }
    }
}
