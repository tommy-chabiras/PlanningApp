<div>
	<h2>Login</h2>
	<form action="/api/user/login" method="post">
		<input type="text" name="username" placeholder="Username" id="username" />
		<input type="text" name="password" placeholder="Password" id="password" />
		<button>Log In</button>
	</form>
</div>

<style>
	h2 {
		font-size: 2rem;
		font-weight: 700;
		text-align: center;
		margin-bottom: 50px;
	}
	form {
		display: flex;
		flex-direction: column;
		gap: 10px;
		align-items: center;
	}
	
	div {
		min-width: max-content;
		padding: 50px;
		border-radius: 25px;
		box-shadow: 0 0 10px 1px rgba(0,0,0);
	}

	input {
		border: 1px solid rgba(88, 88, 88, 0.5);
		background-color: rgb(230 255 250);
		padding: 10px 20px;
	}
	input::placeholder {
		color: #000000;
	}
	button, input {
		border-radius: 50px;
	}

	button {
		margin-top: 15px;
		width: 75%;
		padding: 10px 25px;
		background-color: rgb(199, 241, 190);
	}
</style>
