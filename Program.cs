using System;
using System.Globalization;

public class Program{
    public static void Main(string[] args){
        int numberOfVariables = 0;
        while(numberOfVariables == 0){
            Console.WriteLine("Quantidade de variáveis: ");
            if(int.TryParse(Console.ReadLine(), out int parsedInt) && parsedInt > 0) {
                numberOfVariables = parsedInt;
            }
            Console.Clear();
        }

        Database database = new Database(numberOfVariables);
        Simplex simplex = new Simplex(database);
        simplex.Calculate(true);
        database.PrintDataBase();
        Console.WriteLine("=================================================");
        simplex.printResults();

        while(Console.ReadKey().Key != ConsoleKey.Q){
        }
    }
}