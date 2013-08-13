//  * **************************************************************************
//  * Copyright (c) McCreary, Veselka, Bragg & Allen, P.C.
//  * This source code is subject to terms and conditions of the MIT License.
//  * A copy of the license can be found in the License.txt file
//  * at the root of this distribution. 
//  * By using this source code in any fashion, you are agreeing to be bound by 
//  * the terms of the MIT License.
//  * You must not remove this notice from this software.
//  * **************************************************************************

using System;
using System.Collections;

namespace MvbaMapper
{
	public class EnumerableComparer
	{
		public bool HaveSameContents(IEnumerable listA, IEnumerable listB)
		{
			if (listA == null)
			{
				throw new ArgumentNullException("listA", "enumerable being compared cannot be null");
			}
			var nonNullA = new ArrayList();
			foreach (var item in listA)
			{
				if (item != null)
				{
					nonNullA.Add(item);
				}
			}
			var nonNullB = new ArrayList();
			foreach (var item in listB)
			{
				if (item != null)
				{
					nonNullB.Add(item);
				}
			}

			if (nonNullA.Count != nonNullB.Count)
			{
				return false;
			}
			foreach (var item in nonNullA)
			{
				var index = nonNullB.IndexOf(item);
				if (index == -1)
				{
					return false;
				}
				nonNullB.RemoveAt(index);
			}
			return true;
		}
	}
}