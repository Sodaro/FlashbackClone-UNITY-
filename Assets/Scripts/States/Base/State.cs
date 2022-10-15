/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * NotwithIdle the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEngine;
public abstract class State
{
	protected Player character;
	protected StateMachine stateMachine;
	protected bool action = false;
	protected bool jump = false;
	protected bool downKey = false;
	protected float horizontalInput;

	protected State(Player character, StateMachine stateMachine)
	{
		this.character = character;
		this.stateMachine = stateMachine;
	}

	protected void DisplayOnUI()
	{
		UIManager.Instance.Display(this);
	}
	public virtual void Enter()
	{
		DisplayOnUI();
	}
	public virtual void HandleInput()
	{
		jump = Input.GetKey(Utilities.JumpKey);
		horizontalInput = Input.GetAxisRaw("Horizontal");
		action = Input.GetKey(Utilities.ActionKey);
		downKey = Input.GetKey(Utilities.DownKey);
	}

	public virtual void LogicUpdate()
	{
		//if (character.tilesFallen > 3) //Player can die in any state
		//	stateMachine.ChangeState(character.death);
	}

	public virtual void PhysicsUpdate()
	{

	}

	public virtual void Exit()
	{

	}
}
