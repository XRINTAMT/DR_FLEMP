using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Downloads spritesheets from Google Spreadsheet and saves them to Resources. My laziness made me to create it.
	/// </summary>
	[ExecuteInEditMode]
	public class LocalizationSyncRuntime : MonoBehaviour
	{
		/// <summary>
		/// Table id on Google Spreadsheet.
		/// Let's say your table has the following url https://docs.google.com/spreadsheets/d/1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4/edit#gid=331980525
		/// So your table id will be "1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4" and sheet id will be "331980525" (gid parameter)
		/// </summary>
		public string TableId;

		/// <summary>
		/// Table sheet contains sheet name and id. First sheet has always zero id. Sheet name is used when saving.
		/// </summary>
		public Sheet[] Sheets;

		/// <summary>
		/// Folder to save spreadsheets. Must be inside Resources folder.
		/// </summary>
		//public UnityEngine.Object SaveFolder;

		private const string UrlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
		public List<string> keyList = new List<string>(); // ������ ��� �������� ������

		private void Start()
        {
			Sync();
        }

        /// <summary>
        /// Sync spreadsheets.
        /// </summary>
        public void Sync()
		{
			StopAllCoroutines();
			StartCoroutine(SyncCoroutine2());
		}

		private IEnumerator SyncCoroutine2()
		{
			Debug.Log("<color=yellow>Sync started, please wait for confirmation message...</color>");

			var dict = new Dictionary<string, UnityWebRequest>();
			var downloadedData = new Dictionary<string, string>(); // ������� ��� �������� ������ ��������� ������
			var keyList = new List<string>(); // ������ ��� �������� ������

			foreach (var sheet in Sheets)
			{
				var url = string.Format(UrlPattern, TableId, sheet.Id);

				Debug.Log($"Downloading: {url}...");

				dict.Add(url, UnityWebRequest.Get(url));
			}

			foreach (var entry in dict)
			{
				var url = entry.Key;
				var request = entry.Value;

				if (!request.isDone)
				{
					yield return request.SendWebRequest();
				}

				if (request.error == null)
				{
					var sheet = Sheets.Single(i => url == string.Format(UrlPattern, TableId, i.Id));
					var data = request.downloadHandler.text;

					downloadedData[sheet.Name] = data; // ��������� ������ � �������
					keyList.Add(sheet.Name); // ��������� ���� � ������
					Debug.Log(request.downloadHandler.text);
					ParserColumn(request.downloadHandler.text, 0);

					Debug.LogFormat("Sheet {0} downloaded", sheet.Id);
				}
				else
				{
					throw new Exception(request.error);
				}
			}

			Debug.Log("<color=green>Localization successfully synced!</color>");

			// ������ ������������� ������
			foreach (var key in keyList)
			{
				Debug.LogFormat("Key: {0}", key);
			}
		}

		void ParserColumn(string text,int column) 
		{

			List<string> words = new List<string>();

			// ��������� ����� CSV �� �������
			string[] lines = text.Split('\n');

			// �������� �� ������ ������ CSV, ������� �� ������ ������ (������ ������ - ���������)
			for (int i = 1; i < lines.Length; i++)
			{
				// ��������� ������ CSV �� �������
				string[] columns = lines[i].Split(',');

				// ���������, ��� ���� ���� �� ���� �������
				if (columns.Length > 0)
				{
					// ��������� ����� �� ������� ������� � ������
					words.Add(columns[0]);
				}
			}
            for (int i = 0; i < words.Count; i++)
            {
				Debug.Log(words);
            }
		}




		private IEnumerator SyncCoroutine()
		{
			var folder = Application.streamingAssetsPath;
			//var folder = UnityEditor.AssetDatabase.GetAssetPath(SaveFolder);

			Debug.Log("<color=yellow>Sync started, please wait for confirmation message...</color>");

			var dict = new Dictionary<string, UnityWebRequest>();

			foreach (var sheet in Sheets)
			{
				var url = string.Format(UrlPattern, TableId, sheet.Id);

				Debug.Log($"Downloading: {url}...");

				dict.Add(url, UnityWebRequest.Get(url));
			}

			foreach (var entry in dict)
            {
                var url = entry.Key;
                var request = entry.Value;

				if (!request.isDone)
				{
					yield return request.SendWebRequest();
				}

				if (request.error == null)
				{
					var sheet = Sheets.Single(i => url == string.Format(UrlPattern, TableId, i.Id));
					var path = System.IO.Path.Combine(folder, sheet.Name + ".csv");


					string text = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

					Debug.Log(text);



					System.IO.File.WriteAllBytes(path, request.downloadHandler.data);
					Debug.LogFormat("Sheet {0} downloaded to {1}", sheet.Id, path);
				}
				else
				{
					throw new Exception(request.error);
				}

			}



            //// ������ ������������� ������
            foreach (var entry in dict)
            {
                Debug.LogFormat("Data from sheet {0}: {1}", entry.Key, entry.Value);
            }

            //Debug.Log("<color=green>Localization successfully synced!</color>");
        }
	}
}