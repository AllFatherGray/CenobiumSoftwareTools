using System;
using System.Collections;

namespace Cenobium
{
    public enum MatrixConstraints : byte { NONE = 0, ROW = 2, COL = 4, ROWCOL = 6, HEIGHT = 8, HROW = 10, HCOL = 12, ALL = 14 }
    /// <summary>
    /// 3 dimensional array that uses a single dimensional array underneath
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix<T> : IEnumerable
    {
        uint row, col, height;
        T[] matrixArray;
        public Matrix(uint row, uint col, uint height)
        {
            this.row = row;
            this.col = col;
            this.height = height;
            matrixArray = new T[Length];
        }
        public Matrix(int row, int col, int height)
        {
            this.row = Convert.ToUInt32(row);
            this.col = Convert.ToUInt32(col);
            this.height = Convert.ToUInt32(height);
            matrixArray = new T[Length];
        }
        public uint Length
        {
            get
            {
                return row * col * height;
            }
        }
        public uint Height
        {
            get
            {
                return height;
            }
        }
        public uint Column
        {
            get
            {
                return col;
            }
        }
        public uint Row
        {
            get
            {
                return row;
            }
        }
        public T this[int key1, int key2, int key3 = 0]
        {
            get
            { return matrixArray[((col * key1) + key2) + (key3 * row * col)]; }
            set
            { matrixArray[((col * key1) + key2) + (key3 * row * col)] = value; }
        }
        public T this[uint key1, uint key2, uint key3 = 0]
        {
            get
            { return matrixArray[((col * key1) + key2) + (key3 * row * col)]; }
            set
            { matrixArray[((col * key1) + key2) + (key3 * row * col)] = value; }
        }

        public T this[uint key]
        {
            get
            {
                return matrixArray[key];
            }
            set
            {
                matrixArray[key] = value;
            }
        }
        public T this[int key]
        {
            get
            {
                return matrixArray[key];
            }
            set
            {
                matrixArray[key] = value;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return matrixArray.GetEnumerator();
        }
        public static implicit operator bool(Matrix<T> m)
        {
            return m as object != null;
        }
    }
}