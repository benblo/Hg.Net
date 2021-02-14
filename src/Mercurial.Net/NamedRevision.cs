using System;

namespace Mercurial.Net
{
    /// <summary>
    /// This class is the base class for named revisions, like tags and bookmarks.
    /// </summary>
    public abstract class NamedRevision : IEquatable<NamedRevision>
    {
        /// <summary>
        /// This is the backing field for the <see cref="RevisionNumber"/> property.
        /// </summary>
        private readonly int _RevisionNumber;

        /// <summary>
        /// This is the backing field for the <see cref="Name"/> property.
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedRevision"/> class.
        /// </summary>
        /// <param name="revisionNumber">
        /// The revision number the bookmark or tag is currently for.
        /// </param>
        /// <param name="name">
        /// The name of the tag or bookmark.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is <c>null</c> or empty.</para>
        /// </exception>
        protected NamedRevision(int revisionNumber, string name)
        {
            if (StringEx.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _RevisionNumber = revisionNumber;
            _Name = name;
        }

        /// <summary>
        /// Gets the name of the tag or bookmark.
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Gets the revision number the name is currently for.
        /// </summary>
        public int RevisionNumber
        {
            get
            {
                return _RevisionNumber;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(NamedRevision other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other._RevisionNumber == _RevisionNumber && Equals(other._Name, _Name);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with the current <see cref="object"/>. 
        /// </param>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(NamedRevision))
                return false;
            return Equals((NamedRevision)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_RevisionNumber * 397) ^ (_Name != null ? _Name.GetHashCode() : 0);
            }
        }
    }
}