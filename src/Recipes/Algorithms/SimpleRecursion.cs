namespace Recipes.Algorithms
{
    public static class SimpleRecursion
    {
        // 0! 1,
        // n! = n*(n-1)!, gdzie n nalezy do N, n >= 1
        // Silnia
        public static long Factorial(int depth)
            => depth == 0 ? 1 : depth * Factorial(depth - 1);
        

        // fib(0) = 0
        // fib(1) = 1
        // fib(n) = fib(n-1) + fib(n-2), gdzie n >= 2
        // Fibonacci
        public static long Fib(int x)
            => x < 2 ? x : Fib(x - 1) + Fib(x - 2);
    }
}
