using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                                            "ap-south-1:f1b44440-6440-4aa1-8db8-e93522a00cd2", // Identity Pool ID
                                            _S3Region // Region
                                            ), _S3Region);
            }

            return _s3Client;
        }
    }


    public string bucketName = "serviceappmadfireon";
    public string S3Region = RegionEndpoint.APSouth1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("AWS Manager is null");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);

        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //CognitoAWSCredentials credentials = new CognitoAWSCredentials(
        //                                    "ap-south-1:f1b44440-6440-4aa1-8db8-e93522a00cd2", // Identity Pool ID
        //                                    _S3Region // Region
        //                                    );

        //_s3Client = new AmazonS3Client(credentials, _S3Region);

        //S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        //{
        //    if (responseObject.Exception == null)
        //    {
        //        responseObject.Response.Buckets.ForEach((s3b) =>
        //        {
        //            Debug.Log("Bucket Name: " + s3b.BucketName);
        //        });
        //    }
        //    else
        //    {
        //        Debug.Log("Got AWS Exception: " + responseObject.Exception);
        //    }
        //});
    }

    public void PostObject(string path, string caseID)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = bucketName,
            Key = "case#" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("Successfuly posted to Bucket");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.LogError("Exception occured during uploading: " + responseObj.Exception);
            }
        }
        );
    }

    public void GetList(string caseNumber, Action onComplete = null)
    {
        Debug.Log("AWSMANAGER::GetList()");
        string target = "case#" + caseNumber;

        var request = new ListObjectsRequest()
        {
            BucketName = bucketName
        };

        S3Client.ListObjectsAsync(request, (response) =>
        {
            if (response.Exception == null)
            {
                bool casefound = response.Response.S3Objects.Any(obj => obj.Key == target);

                if (casefound == true)
                {
                    Debug.Log("Found Case File");

                    S3Client.GetObjectAsync(bucketName, target, (responseObj) =>
                      {
                          if (responseObj.Response.ResponseStream != null)
                          {
                              byte[] data = null;

                              using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                              {
                                  using (MemoryStream memory = new MemoryStream())
                                  {
                                      var buffer = new byte[512];
                                      var bytesRead = default(int);

                                      while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                      {
                                          memory.Write(buffer, 0, bytesRead);
                                      }

                                      data = memory.ToArray();
                                  }
                              }

                              using (MemoryStream memory = new MemoryStream(data))
                              {
                                  BinaryFormatter bf = new BinaryFormatter();
                                  Case downloadedCase = bf.Deserialize(memory) as Case;

                                  UIManager.Instance.activeCase = downloadedCase;

                                  if (onComplete != null)
                                      onComplete();
                              }

                          }



                      });

                }
                else
                {
                    Debug.Log("Case File Not Found");
                }
            }
            else
                Debug.LogError("Exception occured during getting list: " + response.Exception);
        });
    }

}
