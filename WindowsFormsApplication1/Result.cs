
namespace WindowsFormsApplication1
{
    public sealed class Result
    {
        private readonly string _recognized;
        private readonly string _remaining;
        private readonly bool _succeeded;

        public Result(string recognized, string remaining, bool succeeded)
        {
            this._recognized = recognized;
            this._remaining = remaining;
            this._succeeded = succeeded;
        }

        public string Recognized
        {
            get { return this._recognized; }
        }

        public string Remaining
        {
            get { return this._remaining; }
        }

        public bool Succeeded
        {
            get { return this._succeeded; }
        }

        public static Result Success(string recognized, string remaining)
        {
            return new Result(recognized, remaining, true);
        }

        public static Result Contact(Result r1, Result r2)
        {
            return new Result(string.Concat(r1.Recognized, r2.Recognized), r2.Remaining, true);
        }

        public static Result Fail()
        {
            return new Result(string.Empty, string.Empty, false);
        }
    }
}
