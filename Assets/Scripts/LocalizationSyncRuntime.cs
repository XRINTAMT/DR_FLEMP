using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.SimpleLocalization
{
	public class LocalizationSyncRuntime : MonoBehaviour
	{
		[SerializeField] GameObject passwordPanel;
		[SerializeField] InputField input;
		public string TableId;
		public Sheet [] Sheets;
		private const string UrlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
		public List<string> keyList = new List<string>();
		public UnityEvent activate;
		public UnityEvent onLoadedSpreadsheets;
		string savedPassword;

        private void Awake()
        {
			savedPassword = PlayerPrefs.GetString("password", "");
        }
        private void Start()
        {
			StartCoroutine(SyncCoroutine());
		}

   

		private IEnumerator SyncCoroutine()
		{
			Debug.Log("<color=yellow>Sync started, please wait for confirmation message...</color>");

			var dict = new Dictionary<string, UnityWebRequest>();
			var keyList = new List<string>(); 

			foreach (var sheet in Sheets)
			{
				var url = string.Format(UrlPattern, TableId, sheet.Id);

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
					ParserColumn(request.downloadHandler.text, 0);
				}
				else
				{
					throw new Exception(request.error);
				}
			}
			onLoadedSpreadsheets.Invoke();
			CheckSavedPassword(savedPassword);
		}

		void ParserColumn(string text,int column) 
		{
			string[] lines = text.Split('\n');

			for (int i = 1; i < lines.Length; i++)
			{
				string[] columns = lines[i].Split(',');

				if (columns.Length > 0)
				{
					keyList.Add(columns[0]);
				}
			}
		}

		public void CheckSavedPassword(string password)
		{

			for (int i = 0; i < keyList.Count; i++)
			{
				if (keyList[i] == password)
				{
					activate?.Invoke();
					return;
				}
			}

			passwordPanel.SetActive(true);

		}
		public void CheckInputPassword() 
		{

            for (int i = 0; i < keyList.Count; i++)
            {
                if (keyList[i] == input.text)
                {
					PlayerPrefs.SetString("password", input.text);

					activate?.Invoke();
					return;

				}
            }

			input.text = "Password is not available";
		}

	}
}