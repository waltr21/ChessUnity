using Amazon;
using Amazon.CognitoIdentity;
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
        try
        {
            User = uc;
            UnityInitializer.AttachToGameObject(temp);
            AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.USWest2;

            //Creds for user with basically no permissions. May need to change to congnito? 
            var cognitoCreds = new CognitoAWSCredentials(AWSIdentity.PoolId, RegionEndpoint.USWest2);
            Client = new AmazonDynamoDBClient(cognitoCreds, RegionEndpoint.USWest2);
        }
        catch (Exception e)
        {
            uc.SetError(e.Message);
        }
    }

    public void LoadServers()
    {
        try
        {


            List<(string, int)> servers = new List<(string, int)>();
            ScanRequest request = new ScanRequest
            {
                TableName = "ServerAdvertisement",
                ProjectionExpression = "IP, Port"
            };

            Client.ScanAsync(request, (res) =>
            {
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
        catch(Exception e)
        {
            User.SetError(e.Message);
        }
    }
}
