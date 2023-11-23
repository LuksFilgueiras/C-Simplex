public class Item{

    public int index {get; set;}
    public string rowKey {get; set;}
    public double value {get; set;}

    public Item(){
        rowKey = "";
    }

    public void printItem(){
        Console.WriteLine("x"+ (index + 1) + ": " + value.ToString("n2"));
    }
}