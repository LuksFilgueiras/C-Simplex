public class Simplex{
    private Database database;
    private List<Item> insertedItems;

    public Simplex(Database database){
        this.database = database;
        this.insertedItems = new List<Item>();
    }

    public void Calculate(bool showMatrix){
        int i = 0;
        while(hasNegativeValueInObjectiveFunction()){
            int columnIndex = getPivotColumnIndex();

            string pivotRowKey = getPivotRowKey(columnIndex);

            double pivotValue = getPivotValue(pivotRowKey, columnIndex);

            double[] newPivotRow = dividePivotRowPerPivotValue(pivotRowKey, pivotValue);

            double[] multipliedRow = multiplyPivotRowPerObjectiveFunctionColumnValue(newPivotRow, columnIndex);

            sumPivotRowWithObjectiveFunctionRow(multipliedRow);

            updateOtherRows(pivotRowKey, columnIndex);

            updateItemsValue();
            if(showMatrix){
                database.PrintDataBase();
                Console.WriteLine("=======================================================");
                Console.ReadLine();
                Console.Clear();
            }
            i++;
        }
    }

    public double[] getObjectiveFunction(){
        return database.databaseDictionary["Z"];
    }

    public double getPivotValue(string pivotRowKey, int columnIndex){
        return database.databaseDictionary[pivotRowKey][columnIndex];
    }

    public int getPivotColumnIndex(){
        double smallerValue = 0;
        double[] zRow = getObjectiveFunction();

        int columnIndex = 0;
        for(int i = 0; i < zRow.Length; i++){
            if(zRow[i] < smallerValue){
                smallerValue = zRow[i];
                columnIndex = i;
            }
        }

        if(columnIndex < database.variablesAmount){
            Item i = new Item();
            i.index = columnIndex;
            insertedItems.Add(i);
        }

        return columnIndex;
    }

    public string getPivotRowKey(int columnIndex){   
        double smallerValue = double.PositiveInfinity;
        string rowKey = "";
        int databaseWidth = getObjectiveFunction().Length;

        foreach(string key in database.databaseKeys){
            if(key == "Z"){
                continue;
            }

            double quotient = database.databaseDictionary[key][databaseWidth - 1] / database.databaseDictionary[key][columnIndex];


            if(quotient < smallerValue && quotient > 0){
                smallerValue = quotient;
                rowKey = key;
            }
        }  

        foreach(Item i in insertedItems){
            if(i.index == columnIndex){
                i.rowKey = rowKey;
            }
        }


        return rowKey;
    }

    public double[] dividePivotRowPerPivotValue(string rowKey, double pivotValue){
        double[] row = database.databaseDictionary[rowKey];
        for(int i = 0; i < row.Length; i++){
            row[i] /= pivotValue;
        }

        return row;
    }

    public double[] multiplyPivotRowPerObjectiveFunctionColumnValue(double[] pivotRow, int columnIndex){
        double[] objectiveFunction = getObjectiveFunction();
        double[] multipliedPivotRow = new double[pivotRow.Length];

        for(int i = 0; i < pivotRow.Length; i++){
            multipliedPivotRow[i] = pivotRow[i];
            multipliedPivotRow[i] *= -objectiveFunction[columnIndex];
        }

        return multipliedPivotRow;
    }

    public double[] sumPivotRowWithObjectiveFunctionRow(double[] pivotRow){
        double[] newObjectiveFunction = getObjectiveFunction();

        for(int i = 0; i < pivotRow.Length; i++){
            newObjectiveFunction[i] += pivotRow[i];
        }

        return newObjectiveFunction;
    }

    public void updateOtherRows(string pivotRowKey, int columnIndex){
        double[] pivotRow = database.databaseDictionary[pivotRowKey];

        foreach(string key in database.databaseKeys){
            if(key == "Z" || key == pivotRowKey){
                continue;
            }

            double oldPivotColumnValue = database.databaseDictionary[key][columnIndex] * -1;

            double[] oldLine = database.databaseDictionary[key];

            double[] updatedRow = new double[pivotRow.Length];

            for(int i = 0; i < pivotRow.Length; i++){
                double newValue = oldLine[i] + (pivotRow[i] * oldPivotColumnValue);
                updatedRow[i] = newValue;
            }
            
            database.databaseDictionary[key] = updatedRow;
        }
    }

    public void updateItemsValue(){
        int lastIndex = getObjectiveFunction().Length - 1;
        Item removedDuplicate = new Item();
        foreach(Item i in insertedItems){
            int iPositionList = insertedItems.IndexOf(i);

            foreach(Item dup in insertedItems){
                if(i.rowKey == dup.rowKey){
                    int dupPositionList = insertedItems.IndexOf(dup);
                    if(dupPositionList > iPositionList){
                        removedDuplicate = i;
                    }
                }
            }
        }

        insertedItems.Remove(removedDuplicate);

        foreach(string key in database.databaseKeys){
            foreach(Item i in insertedItems){
                if(i.rowKey == key){
                    i.value = database.databaseDictionary[key][lastIndex];
                }
            }
        }
    }

    public bool hasNegativeValueInObjectiveFunction(){
        foreach(double d in getObjectiveFunction()){
            if(d < 0){
                return true;
            }
        }
        
        return false;
    }

    public void printResults(){
        double[] objectiveFunction = getObjectiveFunction();
        int lastIndex = objectiveFunction.Length - 1;

        updateItemsValue();

        foreach(Item item in insertedItems){
            item.printItem();
        }

        Console.WriteLine("   ");

        Console.WriteLine("Zmax: " + objectiveFunction[lastIndex]);
    }
}