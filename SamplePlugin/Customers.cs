using System;

using Npgsql;


namespace Incendonet.Plugins.SamplePlugin
{
	public class Customers
	{
		private const string	POSTGRES_SERVER = "postgres";
		private const string	POSTGRES_SBDB = "POSTGRES_SBDB";
		private const string	POSTGRES_SBUSER = "POSTGRES_SBUSER";
		private const string	POSTGRES_SBPASSWORD = "POSTGRES_SBPASSWORD";
		private string			m_connStr = "";
		private Customer		m_cust = null;

		/// <summary>
		/// Customer - Subclass for holding on to values retrieved from DB
		/// </summary>
		private class Customer
		{
			private string m_nameFirst = "";
			private decimal m_balance = 0.0m;
			private DateTime m_due = DateTime.Now;

			public string NameFirst
			{
				get => m_nameFirst;
				set => m_nameFirst = value;
			}

			public decimal Balance
			{
				get => m_balance;
				set => m_balance = value;
			}

			public DateTime Due
			{
				get => m_due;
				set => m_due = value;
			}
		}

		public Customers()
		{
		}

		/// <summary>
		/// Initialization reads DB credentials from environment variables:
		///   POSTGRES_SBDB - The SpeechBridge DB name
		///   POSTGRES_SBUSER, POSTGRES_SBPASSWORD - The credentials
		/// </summary>
		/// <returns></returns>
		public bool Init()
		{
			bool		ret = true;
			string		postgres_sbdb = "", postgres_sbuser = "", postgres_sbpassword = "";

			postgres_sbdb = Environment.GetEnvironmentVariable(POSTGRES_SBDB) ?? "";
			postgres_sbuser = Environment.GetEnvironmentVariable(POSTGRES_SBUSER) ?? "";
			postgres_sbpassword = Environment.GetEnvironmentVariable(POSTGRES_SBPASSWORD) ?? "";
			if( (postgres_sbdb == "") || (postgres_sbuser == "") || (postgres_sbpassword == "") )
			{
				ret = false;
			}
			else
			{
				m_connStr = $"Server={POSTGRES_SERVER};Database={postgres_sbdb};User ID={postgres_sbuser};Password={postgres_sbpassword};";
			}

			return (ret);
		}

		/// <summary>
		/// Given a phone number, loads the customer record.  Returns bool indicating success.
		/// </summary>
		/// <param name="i_phoneNum"></param>
		/// <returns></returns>
		public bool FindByPhonenum(string i_phoneNum)
		{
			bool			bRet = true;
			string			phoneStripped = "";

			if( (m_connStr == "") || ("" == (i_phoneNum ?? "")) )
			{
				bRet = false;
			}
			else
			{
				if ((i_phoneNum.Length == 11) && (i_phoneNum.StartsWith("1")))
				{
					phoneStripped = i_phoneNum.Substring(1);
				}
				else
				{
					phoneStripped = i_phoneNum;
				}

				try
				{
					using (NpgsqlConnection conn = new NpgsqlConnection(m_connStr))
					{
						conn.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT NameFirst, Balance, DueDate FROM tblCustomers WHERE PhoneMobile = (@phoneStripped) OR PhoneHome = (@phoneStripped) OR PhoneWork = (@phoneStripped) OR PhoneWorkExt = (@phoneStripped)", conn))
						{
							cmd.Parameters.AddWithValue("phoneStripped", phoneStripped);
							using (NpgsqlDataReader reader = cmd.ExecuteReader())
							{
								if(!reader.HasRows)
								{
									bRet = false;
								}
								else
								{
									m_cust = new Customer();

									// Get the first row's values
									reader.Read();

									m_cust.NameFirst = reader.GetString(0);
									m_cust.Balance = reader.GetDecimal(1);
									m_cust.Due = reader.GetDateTime(2);
								}
							}
						}
					}
				}
				catch(Exception exc)
				{
					Console.Error.WriteLine($"{DateTime.Now} SamplePlugin.FindByPhonenum Caught exception: '{exc.ToString()}'.");
					bRet = false;
				}
			}

			return (bRet);
		}

		// Accessor members
		public string GetFirstName() => m_cust.NameFirst;
		public string GetBalance() => $"{m_cust.Balance.ToString()} dollars";
		public string GetDueDate() => m_cust.Due.ToLongDateString();
	}
}
