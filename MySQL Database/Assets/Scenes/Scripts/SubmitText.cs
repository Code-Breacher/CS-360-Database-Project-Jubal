using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SubmitText : MonoBehaviour
{
    private const string V = "σ";
    public string ans;
    public string inputstring;
    public InputField submitfield;
    public InputField Connectionfield;
    public InputField Usernamefield;
    public InputField Passwordfield;
    public InputField Databasefield;

    public Button myButton;
    public Button selectButton;
    public Button UnionButton;
    public Button IntersectionButton;
    public Button ProjectionButton;
    public Button CartesianButton;
    //public Button JoinButton;
    public Button NatButton;
    public Button NEQButton;
    public Button AndButton;
    public Button ORButton;

    public string connection = "localhost";
    public string password = "";
    public string database = "testdb";
    public string username = "root"; 

    // Start is called before the first frame update
    void Start()
    {
        //Get all the buttons and text set up
        Connectionfield.text = connection;
        Usernamefield.text = username;
        Passwordfield.text = password;
        Databasefield.text = database;
        myButton.onClick.AddListener(ProcessText);
        selectButton.onClick.AddListener(AddSelect);
        UnionButton.onClick.AddListener(AddUnion);
        IntersectionButton.onClick.AddListener(AddIntersection);
        ProjectionButton.onClick.AddListener(AddProjection);
        CartesianButton.onClick.AddListener(AddCartesian);
        //JoinButton.onClick.AddListener(AddJoin);
        NatButton.onClick.AddListener(AddNat);
        NEQButton.onClick.AddListener(AddNEQ);
        AndButton.onClick.AddListener(AddAnd);
        ORButton.onClick.AddListener(AddOR);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //----------------------Get the text from all the inputfields---------------call PHP code
    public void ProcessText()
    {
        inputstring = submitfield.text;
        ParseString(inputstring);
        //Debug.Log("Process input:" + inputstring); --------Repeat the input of the user
        WWWForm form = new WWWForm();
        form.AddField("request", ans);
        form.AddField("connection", Connectionfield.text);
        form.AddField("username", Usernamefield.text);
        form.AddField("password", Passwordfield.text);
        form.AddField("database", Databasefield.text);
        //change http://localhost/datasick/login.php to your own
        WWW w = new WWW("http://localhost/Databases/ProcessRequest.php", form);
        StartCoroutine(processing(w));
    } //----
    //-------------Use the PHP code for connecting, and running SQL code-----------
    IEnumerator processing(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            if (w.text == "login-SUCCESS")
            {
                Debug.Log("LogedIn");
                WWWForm form2 = new WWWForm();
                //change http://localhost/datasick/GetUserData.php to your own
                WWW urlData = new WWW("http://localhost/Databases/ProcessRequest.php", form2);
            }
            else
            {
                Debug.Log(w.text);
            }
        }
        else
        {
            Debug.Log("ERROR: " + w.error + "\n");
        }
    }
    //------------------Parse the string and Translate it to SQL---------------
    public void ParseString(string input)
    {
        int length = input.Length;
        //Debug.Log(length);
        string[] ary;
        ary = new string[10000];
        int i = 0;
        bool found = false;
        bool foundPro = false;

        foreach (char c in input)
        {
            if (c.Equals('σ'))
            {
                found = true;
                //Debug.Log("found");
            }
            else if (c.Equals('{') && found == true)
            {

            }
            else if (c.Equals('}') && found == true)
            {
                i++;
            }
            else if (c.Equals('(') && found == true)
            {
                
            }
            else if (c.Equals('∧'))
            {
                ary[i] = ary[i] + " AND ";
            }
            else if (c.Equals('∨'))
            {
                ary[i] = ary[i] + " OR ";
            }
            else if (c.Equals('≠'))
            {
                ary[i] = ary[i] + " <> ";
            }
            else if (c.Equals(')') && found == true)
            {
                string tmp = ary[i];
                string tmp2 = ary[i - 1];
                ary[i - 1] = "select * FROM ";
                ary[i - 1] = ary[i - 1] + tmp;
                ary[i] = " WHERE ";
                ary[i] = ary[i] + tmp2;
            }
            else if (c.Equals('∪'))
            {
                while((string.Equals(ary[i], " ")) && i > 0)
                {
                    i--;
                }
                string tmp = ary[i];
                ary[i] = "select * FROM ";
                ary[i] = ary[i] + tmp;
                ary[i] = ary[i] + " UNION select * FROM ";
                i++;
                while ((string.Equals(ary[i], " ")))
                {
                    i++;
                }

            }
            else if (c.Equals('∩'))
            {
                while ((string.Equals(ary[i], " ")) && i > 0)
                {
                    i--;
                }
                string tmp = ary[i];
                ary[i] = "select * FROM ";
                ary[i] = ary[i] + tmp;
                ary[i] = ary[i] + " INTERSECT select * FROM ";
                i++;
                while ((string.Equals(ary[i], " ")))
                {
                    i++;
                }
            }
            else if (c.Equals('Π'))
            {
                foundPro = true; 
            }
            else if (c.Equals('{') && foundPro == true)
            {

            }
            else if (c.Equals('}') && foundPro == true)
            {
                i++;
            }
            else if (c.Equals('(') && foundPro == true)
            {

            }
            else if (c.Equals(')') && foundPro == true)
            {
                string tmp = ary[i];
                string tmp2 = ary[i - 1];
                ary[i - 1] = "select ";
                ary[i - 1] = ary[i - 1] + tmp2;
                ary[i] = " FROM ";
                ary[i] = ary[i] + tmp;
            }
            else if (c.Equals('×'))
            {
                while ((string.Equals(ary[i], " ")) && i > 0)
                {
                    i--;
                }
                string tmp = ary[i];
                ary[i] = "select * FROM ";
                ary[i] = ary[i] + tmp;
                ary[i] = ary[i] + ", ";
                i++;
                while ((string.Equals(ary[i], " ")))
                {
                    i++;
                }
            }
            else if (c.Equals('⋈'))
            {
                while ((string.Equals(ary[i], " ")) && i > 0)
                {
                    i--;
                }
                string tmp = ary[i];
                ary[i] = "select * FROM ";
                ary[i] = ary[i] + tmp;
                ary[i] = ary[i] + " NATURAL JOIN ";
                i++;
                while ((string.Equals(ary[i], " ")))
                {
                    i++;
                }
            }
            else if (c.Equals(' '))
            {
                i++;
                ary[i] = ary[i] + c;
            }
            else
            {
                ary[i] = ary[i] + c;
            }
        }
        ans = string.Join("", ary);
        Debug.Log(ans);
    }
    //-----------------Add the corresponding symbol for the button pressed-----------------
    public void AddSelect()
    {
        submitfield.text = submitfield.text + "σ{}()";
    }

    public void AddUnion()
    {
        submitfield.text = submitfield.text + "∪";
    }

    public void AddIntersection()
    {
        submitfield.text = submitfield.text + "∩";
    }

    public void AddProjection()
    {
        submitfield.text = submitfield.text + "Π{}()";
    }

    public void AddCartesian()
    {
        submitfield.text = submitfield.text + "×";
    }

    public void AddJoin()
    {
        submitfield.text = submitfield.text + "Join";
    }

    public void AddNat()
    {
        submitfield.text = submitfield.text + "⋈";
    }

    public void AddNEQ()
    {
        submitfield.text = submitfield.text + "≠";
    }

    public void AddAnd()
    {
        submitfield.text = submitfield.text + "∧";
    }

    public void AddOR()
    {
        submitfield.text = submitfield.text + "∨";
    }
}
