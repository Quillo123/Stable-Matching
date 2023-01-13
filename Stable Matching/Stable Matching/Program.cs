

Console.WriteLine("Stable Matching Algorithm Sample");

Console.WriteLine("File Path: ");
string filename = Console.ReadLine();

string[] lines = File.ReadAllLines(filename);

//Taking in count to guarantee O(1) complexity for Dictionary Methods 
int count = int.Parse(lines[0]);

Dictionary<string, Man> freeMen = new Dictionary<string, Man>(count);
Dictionary<string, Man> takenMen = new Dictionary<string, Man>(count);
Dictionary<string, Woman> women = new Dictionary<string, Woman>(count);
for(int i = 0; i < count; i++)
{
    string l = lines[i + 1];
    Man p = new Man(l);
    freeMen.Add(p.id, p);
}
for (int i = 0; i < count; i++)
{
    string l = lines[i + 1 + count];
    Woman p = new Woman(l);
    women.Add(p.id, p);
}



//Stable Matching Algorithm
while(freeMen.Count > 0) { 
    var man = freeMen.First().Value;
    string w = man.nextPreferrence();

    if (women[w].isFree)
    {
        women[w].CurrentPartner = man.id;
        freeMen.Remove(man.id);                         //O(1)
        takenMen.Add(man.id, man);                      //O(1)
    }
    else if (women[w].PrefersToCurrent(man.id))         //O(n)
    {
        var dumped = takenMen[women[w].CurrentPartner];
        freeMen.Add(dumped.id, dumped);                 //O(1)
        takenMen.Remove(dumped.id);                     //O(1)

        women[w].CurrentPartner = man.id;
        freeMen.Remove(man.id);                         //O(1)
        takenMen.Add(man.id, man);                      //O(1)
        
    }
    
}

foreach(var man in takenMen)
{
    Console.WriteLine(man.Value.id + " <=> " + man.Value.CurrentPartner);
}

public class Woman
{

    List<string> preferences = new List<string>();

    public string CurrentPartner
    {
        get { return currentPartner; }
        set {
            currentPartner = value;
            if(value == "")
            {
                isFree = true;
            }
            else
            {
                isFree = false;
            }
        }
    }
    string currentPartner = "";

    public string id = "";

    public Woman(string s)
    {
        LoadWomanFromString(s);
    }

    public bool isFree = true;

    public bool PrefersToCurrent(string A)
    {
        for (int i = 0; i < preferences.Count; i++)
        {
            if (preferences[i] == A)
            {
                return true;
            }
            else if (preferences[i] == currentPartner)
            {
                return false;
            }
        }

        Console.WriteLine("Person \"" + A + "\" were found in \"" + id + "\" preferences");
        return false;
    }

    public bool LoadWomanFromString(string s)
    {
        string[] val = s.Split(' ');
        id = val[0];

        for (int i = 1; i < val.Length; i++)
        {
            preferences.Add(val[i]);
        }

        return true;
    }
}

public class Man
{

    List<string> preferences = new List<string>();
    public int curr = -1;
    public string CurrentPartner
    {
        get
        {
            if(curr < 0)
            {
                return "";
            }
            return preferences[curr];
        }
    }

    public string id = "";

    public string nextPreferrence()
    {


        if(curr >= preferences.Count - 1)
        {
            return "";
        }
        curr++;
        return preferences[curr];
    }

    public Man(string s)
    {
        LoadManFromString(s);
    }

    public bool isFree = true;

    public bool PrefersToCurrent(string A, out int index)
    {
        for(int i = curr; i < preferences.Count; i++)
        {
            if(preferences[i] == A)
            {
                index = i;
                return true;
            }
            else if (preferences[i] == CurrentPartner)
            {
                index = -1;
                return false;
            }
        }

        Console.WriteLine("Person \"" + A + "\" were found in \"" + id + "\" preferences");
        index = -1;
        return false;
    }

    public bool LoadManFromString(string s)
    {
        string[] val = s.Split(' ');
        id = val[0];

        for(int i = 1; i < val.Length; i++)
        {
            preferences.Add(val[i]);
        }

        return true;
    }
}