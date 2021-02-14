using System;
using System.Globalization;

namespace Mercurial.Net
{
    /// <summary>
    /// This class encapsulates information about a single persistent tag in the repository.
    /// </summary>
    public class Tag : NamedRevision, IEquatable<Tag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
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
        public Tag(int revisionNumber, string name)
            : base(revisionNumber, name)
        {
            // Do nothing here
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
        public bool Equals(Tag other)
        {
            return base.Equals(other);
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
            return Equals(obj as Tag);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current <see cref="object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Tag (Name={0}, Revision=#{1})", Name, RevisionNumber);
        }
    }
}