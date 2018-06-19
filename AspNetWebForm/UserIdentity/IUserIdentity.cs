using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetWebForm.UserIdentity
{
    public interface IUserIdentity
    {
        string AuthenticationType { get; }
        bool IsAuthenticated { get; }
        string Name { get; }
    }
    public class UserClaim
    {
        public string Type { get; private set; }
        public string Value { get; private set; }
        public UserClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
    public class UserClaimsIdentity : IUserIdentity
    {
        private List<UserClaim> _claims;
        public UserClaimsIdentity()
        {

        }
        public UserClaimsIdentity(IEnumerable<UserClaim> claims):this(null,claims,string.Empty){}
        public UserClaimsIdentity(IUserIdentity identity, IEnumerable<UserClaim> claims):this(identity,claims,string.Empty){}
        public UserClaimsIdentity(IUserIdentity identity, IEnumerable<UserClaim> claims,string roleClaimsType)
        {
            _claims = new List<UserClaim>();
            UserClaim claimsIdentity = identity as UserClaim;
            if (claimsIdentity != null)
                _claims.Add(claimsIdentity);
            Claims = claims;
        }
        public virtual IEnumerable<UserClaim> Claims { get; private set; }
        public virtual string Name { get; private set; }
        public virtual bool IsAuthenticated { get; private set; }
        public virtual void AddClaim(UserClaim claim)
        {
            _claims.Add(claim);
        }
        public virtual void AddClaims(IEnumerable<UserClaim> claims)
        {
            _claims.AddRange(claims);
        }
        public virtual IEnumerable<UserClaim> FindAll(Predicate<UserClaim> match)
        {
            return _claims.FindAll(match);
        }
        public virtual bool HasClaim(Predicate<UserClaim> match)
        {
            return _claims.Exists(match);
        }
        public virtual void RemoveClaim(UserClaim claim)
        {
            _claims.Remove(claim);
        }
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }
        public string RoleClaimType { get; set; }
    }
    public interface IUserPrincipal
    {
        IUserIdentity Identity { get; }

        bool IsRole(string role);
    }
    public class UserClaimsPrincipal : IUserPrincipal
    {
        private List<UserClaimsIdentity> _identities;
        private IUserIdentity _identity;
        public UserClaimsPrincipal() { }
        public UserClaimsPrincipal(IEnumerable<UserClaimsIdentity> identities)
        {
            Identities = identities;
        }
        public UserClaimsPrincipal(IUserIdentity identity)
        {
            _identity = identity;
        }
        public IUserIdentity Identity
        {
            get
            {
                return _identity;
            }
            private set
            {
            }
        }
        public IEnumerable<UserClaimsIdentity> Identities
        {
            get
            {
                _identities = new List<UserClaimsIdentity>();
                return _identities;
            }
            private set
            {
            }
        }
        public IEnumerable<UserClaim> Claims
        {
            get
            {
                foreach (UserClaimsIdentity current in _identities)
                {
                    foreach (UserClaim item in current.Claims)
                    {
                        yield return item;
                    }
                }
            }
        }
     
        public virtual bool IsRole(string role)
        {
            foreach(UserClaimsIdentity current in _identities)
            {
                if (current.RoleClaimType.Contains(role))
                    return true;
            }
            return false;
        }
        public virtual void AddIdentities(IEnumerable<UserClaimsIdentity> identities)
        {
            _identities.AddRange(identities);
        }
        public virtual void AddIdentity(UserClaimsIdentity identity)
        {
            _identity = identity;
        }
        public virtual IEnumerable<UserClaim> FindAll(Predicate<UserClaim> match)
        {
            foreach (UserClaimsIdentity current in _identities)
            {
                foreach (UserClaim item in current.FindAll(match))
                {
                    yield return item;
                }
            }
        }
        public virtual bool HasClaim(Predicate<UserClaim> match)
        {
            foreach (UserClaimsIdentity current in _identities)
            {
                foreach (UserClaim item in current.FindAll(match))
                    return true;
            }
            return false;
        }
    }
}