using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SafeResult;

namespace UnitTest
{
	[TestFixture]
	public class SafeResult
	{
		[Test]
		public void shouldMapErrorsAndKeepInners(){
			SafeResult<bool> firstResult = new SafeResult<bool>();
			
			firstResult.logError("test error");
			
			SafeResult<bool> secondResult = new SafeResult<bool>();
			
			Assert.AreEqual(1,firstResult.Errors.Count);
			
			secondResult.mapErrors(firstResult);
			
			Assert.AreEqual(1,secondResult.Errors.Count);
			Assert.AreEqual(1,secondResult.Inner.Errors.Count);
			
			Assert.AreEqual(1,firstResult.Errors.Count);
		}
		
		[Test]
		public void shouldMapStatusAndKeepInners(){
			SafeResult<bool> firstResult = new SafeResult<bool> { Status = SAFE_RESULT.Success };
			
			SafeResult<bool> secondResult = new SafeResult<bool>();
			
			Assert.AreEqual(SAFE_RESULT.Success, firstResult.Status);
			
			secondResult.mapStatus(firstResult);
			
			Assert.AreEqual(SAFE_RESULT.Success, secondResult.Inner.Status);
			
			Assert.AreEqual(SAFE_RESULT.Success, secondResult.Status);
		}
		
		[Test]
		public void shouldChainMapping(){
			SafeResult<bool> firstResult = new SafeResult<bool>{ Status = SAFE_RESULT.Success };
			
			firstResult.logError("test error");
			
			SafeResult<bool> secondResult = new SafeResult<bool>();
			
			Assert.AreEqual(1,firstResult.Errors.Count);
			Assert.AreEqual(SAFE_RESULT.Success, firstResult.Status);
			
			secondResult.mapErrors(firstResult).mapStatus();
			
			//check outer
			Assert.AreEqual(1,secondResult.Errors.Count);
			Assert.AreEqual(SAFE_RESULT.Success, secondResult.Status);
			
			//check inner
			Assert.AreEqual(1,secondResult.Inner.Errors.Count);
			Assert.AreEqual(SAFE_RESULT.Success, secondResult.Inner.Status);
			
			Assert.AreEqual(1,firstResult.Errors.Count);
		}
		
		[Test]
		public void shouldChangeSuccessIfErrors(){
			SafeResult<bool> firstResult = new SafeResult<bool>{ Status = SAFE_RESULT.Success };
			
			firstResult.logError("test error");
			
			firstResult.changeSuccessIfErrors();
			
			//check status
			Assert.AreEqual(SAFE_RESULT.ConditionalSuccess, firstResult.Status);
		}
		
		[Test]
		public void shouldMapResult(){
			SafeResult<bool> firstResult = new SafeResult<bool> { Result = false };
			
			firstResult.logError("test error");
			
			SafeResult<bool> secondResult = new SafeResult<bool> { Result = true };
			
			Assert.IsFalse(firstResult.Result);
			
			firstResult.mapResult(secondResult);
			
			Assert.IsTrue(firstResult.Result);
		}
	}
}
