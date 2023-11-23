using System.Globalization;
public class Database{

    public List<string> databaseKeys {get; private set;}
    public Dictionary<string, double[]> databaseDictionary {get; set;}
    public int variablesAmount;

    public Database(int variablesAmount){
        databaseKeys = new List<string>();
        databaseDictionary = new Dictionary<string, double[]>();
        this.variablesAmount = variablesAmount;
        SetDataBase();
    }

    public void SetDataBase(){
        string rootPath = AppDomain.CurrentDomain.BaseDirectory;

        try{
            string[] lines = File.ReadAllLines(rootPath + @"\Planilhas\BD_NOVO.csv");
            SetDatabaseKeys(lines);

            foreach(string line in lines){
                string[] line_split = line.Split(',');

                double[] line_values = new double[line_split.Length - 1];
        
                int line_values_index = 0;

                for(int i = 1; i < line_split.Length; i++){
                    line_values[line_values_index] = double.Parse(line_split[i], CultureInfo.InvariantCulture);
                    line_values_index++;
                } 

                databaseDictionary.Add(line_split[0], line_values);
            }
        }
        catch(IOException e){
            Console.WriteLine(e.Message);
        }
    }

    private void SetDatabaseKeys(string[] lines){
        foreach(string line in lines){
            databaseKeys.Add(line.Split(',')[0]);
        }
    }

    public void AddToDataBase(string key, double[] values){
        databaseKeys.Add(key);
        databaseDictionary.Add(key, values);
    }

    public void PrintDataBase(){
        if(databaseDictionary == null){
            throw new NullReferenceException("Missing database!");
        }

        if(databaseKeys == null){
            throw new NullReferenceException("Missing database keys!");
        }

        foreach(string key in databaseKeys){
            //Console.Write(key);
            foreach(double value in databaseDictionary[key]){
                if(value % 1 == 0 || value == 0){
                    Console.Write("|" + value.ToString("n0"));
                }else{
                    Console.Write("|" + value.ToString("n2"));
                }
            }
            Console.WriteLine();
        }
    }
}