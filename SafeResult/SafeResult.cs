using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SafeResult
{
	public enum SAFE_RESULT{
		Success,
		Failure,
		ConditionalSuccess
	}
	
	public interface ResultSubset{
		List<string> Errors {get;set;}
		SAFE_RESULT Status {get;set;}
	}
	
	public class SafeResult<T> : ResultSubset
	{
		private ResultSubset innerResult = null;
			
		public SafeResult ()
		{
			this.Errors = new List<string>();
			this.Status = SAFE_RESULT.Failure;
			this.Result = default(T);
		}
		
		public SafeResult(ResultSubset someInner){
			innerResult = someInner;
			Errors.AddRange(someInner.Errors);
			Status = someInner.Status;
		}
		
		/// <summary>
		/// Inner result used for mapping.  Allows chaining of calls.
		/// </summary>
		public ResultSubset Inner {get {return innerResult;} set{innerResult = value;}}
		public List<string> Errors {get;set;}
		public SAFE_RESULT Status {get;set;}
		public T Result {get;set;}
		
		/// <summary>
		/// Adds an error entry to the Errors collection
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="args">
		/// A <see cref="System.Object[]"/>
		/// </param>
		public void logError(string msg, params object[] args){
			Errors.Add(String.Format(msg,args));
		}
		public SafeResult<T> mapErrors(ResultSubset someInner){
			innerResult = someInner;
			return mapErrors();
		}
		public SafeResult<T> mapErrors(){
			if(innerResult != null){
				Errors.AddRange(innerResult.Errors);			
			}
			
			return this;
		}
	
		public SafeResult<T> mapStatus(ResultSubset someInner){
			innerResult = someInner;
			return mapStatus();
		}
		public SafeResult<T> mapStatus(){
			if(innerResult != null){
				this.Status = innerResult.Status;
			}
			
			return this;
		}
		
		public bool IsSuccess { get { return Status == SAFE_RESULT.Success;}}
		public bool IsFailure { get { return Status == SAFE_RESULT.Failure;}}
		public bool IsCondSuccess { get { return Status == SAFE_RESULT.ConditionalSuccess;}}
		
		public SafeResult<T> changeSuccessIfErrors(){
			if(Status == SAFE_RESULT.Success && Errors.Count > 0){
				Status = SAFE_RESULT.ConditionalSuccess;
			}
			
			return this;
		}
	
		public SafeResult<T> mapResult(SafeResult<T> someInner){
			innerResult = someInner;
			Result = someInner.Result;
			
			return this;
		}
	}
}

