//
// System.Security.Cryptography.X509Certificates.X509CertificateCollection
//
// Authors:
//	Lawrence Pit (loz@cable.a2000.nl)
//	Sebastien Pouliot (spouliot@motus.com)
//
// Copyright (C) 2004 Novell (http://www.novell.com)
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;

namespace System.Security.Cryptography.X509Certificates {

[Serializable]
public class X509CertificateCollectionMono : CollectionBase {
	
	public X509CertificateCollectionMono()
	{
	}
	
	public X509CertificateCollectionMono(X509CertificateMono[] value) 
	{
		AddRange (value);
	}
	
	public X509CertificateCollectionMono(X509CertificateCollectionMono value)
	{
		AddRange (value);
	}
	
	// Properties
	
	public X509CertificateMono this [int index] {
		get { return (X509CertificateMono) InnerList [index]; }
		set { InnerList [index] = value; }
	}
	
	// Methods

	public int Add (X509CertificateMono value)
	{
		if (value == null)
			throw new ArgumentNullException ("value");
		
		return InnerList.Add (value);
	}
	
	public void AddRange (X509CertificateMono[] value) 
	{
		if (value == null)
			throw new ArgumentNullException ("value");

		for (int i = 0; i < value.Length; i++) 
			InnerList.Add (value [i]);
	}
	
	public void AddRange (X509CertificateCollectionMono value)
	{
		if (value == null)
			throw new ArgumentNullException ("value");

		for (int i = 0; i < value.InnerList.Count; i++) 
			InnerList.Add (value [i]);
	}
	
	public bool Contains (X509CertificateMono value) 
	{
		if (value == null)
			return false;

		byte[] hash = value.GetCertHash ();
		for (int i=0; i < InnerList.Count; i++) {
			X509Certificate x509 = (X509Certificate) InnerList [i];
			if (Compare (x509.GetCertHash (), hash))
				return true;
		}
		return false;
	}

	public void CopyTo (X509CertificateMono[] array, int index)
	{
		InnerList.CopyTo (array, index);
	}
	
	public new X509CertificateEnumerator GetEnumerator ()
	{
		return new X509CertificateEnumerator (this);
	}
	
	public override int GetHashCode () 
	{
		return InnerList.GetHashCode ();
	}
	
	public int IndexOf (X509CertificateMono value)
	{
		return InnerList.IndexOf (value);
	}
	
	public void Insert (int index, X509CertificateMono value)
	{
		InnerList.Insert (index, value);
	}
	
	public void Remove (X509CertificateMono value)
	{
		if (value == null)
			throw new ArgumentNullException ("value");
		if (IndexOf (value) == -1) {
			throw new ArgumentException ("value", 
				Locale.GetText ("Not part of the collection."));
		}

		InnerList.Remove (value);
	}

	// private stuff

	private bool Compare (byte[] array1, byte[] array2) 
	{
		if ((array1 == null) && (array2 == null))
			return true;
		if ((array1 == null) || (array2 == null))
			return false;
		if (array1.Length != array2.Length)
			return false;
		for (int i=0; i < array1.Length; i++) {
			if (array1 [i] != array2 [i])
				return false;
		}
		return true;
	}

	// Inner Class
	
	public class X509CertificateEnumerator : IEnumerator {

		private IEnumerator enumerator;

		// Constructors
		
		public X509CertificateEnumerator (X509CertificateCollectionMono mappings)
		{
			enumerator = ((IEnumerable) mappings).GetEnumerator ();
		}

		// Properties
		
		public X509CertificateMono Current {
			get { return (X509CertificateMono) enumerator.Current; }
		}
		
		object IEnumerator.Current {
			get { return enumerator.Current; }
		}

		// Methods
		
		bool IEnumerator.MoveNext ()
		{
			return enumerator.MoveNext ();
		}
		
		void IEnumerator.Reset () 
		{
			enumerator.Reset ();
		}
		
		public bool MoveNext () 
		{
			return enumerator.MoveNext ();
		}
		
		public void Reset ()
		{
			enumerator.Reset ();
		}
	}		
}

}

