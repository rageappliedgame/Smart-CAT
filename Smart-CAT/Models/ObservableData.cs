using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthAssessmentWizard
{

    //Data.AllGameLogs.Item1 = Observables
    //Data.AllGameLogs.Item2 = Columns of Data for each Observable

    //item1	item2
    //o1		[12,23...]
    //o2		[22,33,44...]
    public class Observables<T> : List<ObservableData<T>>
    {
#warning Why is this a String and not a Double?
    }
    public class ObservableData<T>
    {
        private T[] arr = Array.Empty<T>();

        public String ObservableName { get; private set; }

        /// <summary>
        /// Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        ///
        /// <param name="i"> Zero-based index of the entry to access. </param>
        ///
        /// <returns>
        /// The indexed item.
        /// </returns>
        public T this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="size"> The size. </param>
        public ObservableData(int size, string name)
        {
            this.ObservableName = name;
            arr = new T[size];
        }
    }

}
