using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorGame.Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		public float walkSpeed;
		public float runSpeed;

		public float playerJumpForce;
		public ForceMode appliedForceMode;

		private bool isJumping;
		public float currentSpeed;

		private float _xAxis;
		private float _zAxis;
		private Rigidbody _rb;
		private RaycastHit _hit;
		private Vector3 _groundLocation;
		private bool _isShiftPressedDown;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			_xAxis = Input.GetAxis("Horizontal");
			_zAxis = Input.GetAxis("Vertical");

			currentSpeed = _isShiftPressedDown ? runSpeed : walkSpeed;

			isJumping = Input.GetButtonDown("Jump");

			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10f, Color.blue);
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, Mathf.Infinity))
			{
				if(String.Compare(_hit.collider.tag, "Ground", StringComparison.Ordinal) == 0)
				{
					_groundLocation = _hit.point;
				}

				var distanceFromPlayerToGround = Vector3.Distance(transform.position, _groundLocation);

				if(distanceFromPlayerToGround > 1f)
				{
					isJumping = false;
				}
			}
		}

		private void FixedUpdate()
		{
			_rb.MovePosition(transform.position + Time.fixedDeltaTime * currentSpeed * transform.TransformDirection(_xAxis, 0f, _zAxis));

			if (isJumping)
			{
				PlayerJump(playerJumpForce, appliedForceMode);
			}
		}

		private void OnGUI()
		{
			_isShiftPressedDown = Event.current.shift;
		}

		private void PlayerJump(float jumpForce, ForceMode forceMode)
		{
			_rb.AddForce(jumpForce * _rb.mass * Time.deltaTime * Vector3.up, forceMode);
		}
	}
}