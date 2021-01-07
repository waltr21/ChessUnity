using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Assets.Scripts.ServerClient;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class DBConnection
{
    AmazonDynamoDBClient Client;
    Table ServerTable;
    UserClient User;

    // Start is called before the first frame update
    public DBConnection(GameObject temp, UserClient uc)
    {
        User = uc;
        UnityInitializer.AttachToGameObject(temp);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
        clientConfig.RegionEndpoint = RegionEndpoint.USWest2;

        var creds = GetAWSCreds();
        //Creds for user with basically no permissions. May need to change to congnito? 
        BasicAWSCredentials awsCredentials = new BasicAWSCredentials(creds.Item1, creds.Item2);
        Client = new AmazonDynamoDBClient(awsCredentials, RegionEndpoint.USWest2);
    }

    private (string,string) GetAWSCreds()
    {
        try
        {
            var doc = new XmlDocument();
            doc.Load("Assets\\Scripts\\ServerClient\\AWS_Creds.config");
            XmlElement ak = (XmlElement) doc.SelectSingleNode(string.Format("configuration/AWSSettings/add[@key='{0}']", "AccessKey"));
            XmlElement sk = (XmlElement)doc.SelectSingleNode(string.Format("configuration/AWSSettings/add[@key='{0}']", "SecretKey"));
            return (ak.Attributes["value"].Value, sk.Attributes["value"].Value);
        }
        catch (System.IO.FileNotFoundException e)
        {
            throw new Exception("No configuration file found.", e);
        }
    }

    public void ListTables()
    {
        var request = new ListTablesRequest
        {
            ExclusiveStartTableName = "ServerAdvertisement",
            Limit = 1 // Page size.
        };

        TableConfig request2 = new TableConfig("ServerAdvertisement");

        Table.LoadTableAsync(Client, "ServerAdvertisement", (res) => { 
            ServerTable = res.Result;
            LoadServers();
        });
    }

    public void LoadServers()
    {
        List<(string, int)> servers = new List<(string, int)>();
        ScanRequest request = new ScanRequest
        {
            TableName = "ServerAdvertisement",
            ProjectionExpression = "IP, Port"
        };

        Client.ScanAsync(request, (res) => {
            foreach (Dictionary<string, AttributeValue> item in res.Response.Items)
            {
                string ip = "";
                int port = 0;
                foreach (var kvp in item)
                {
                    if (kvp.Value.S != null) ip = kvp.Value.S;
                    if (kvp.Value.N != null) port = Int32.Parse(kvp.Value.N);
                }
                (string, int) t = (ip, port);
                servers.Add(t);
                Debug.Log(t);
            }
            User.ProcessServers(servers);
        });
    }
}
