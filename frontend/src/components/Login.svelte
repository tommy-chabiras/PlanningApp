<script lang="ts">
	let username = "";
	let password = "";
	let error = false;
	let errorStream: {error?: String};

	async function login(e: Event) {
		e.preventDefault();
		const response = await fetch("/api/user/login", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ username, password }),
		});
		
		if (!response.ok) {
			errorStream = JSON.parse(await response.text());
			error = true;
		}
		else {
			const data = await response.json();
			localStorage.setItem("token", data.token);
			window.location.href = "/";
		}
	}
</script>

<div>
	<h2>Login</h2>
	{#if error}
		<p>{errorStream.error}</p>
	{/if}
	<form on:submit={login} method="post">
		<input type="text" bind:value={username} placeholder="Username" />
		<input type="password" bind:value={password} placeholder="Password" />
		<button>Log In</button>
	</form>
</div>

<style>
	p {
		color: red;
		text-align: center;
	}
	
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
		box-shadow: 0 0 10px 1px rgba(0, 0, 0);
	}

	input {
		border: 1px solid rgba(88, 88, 88, 0.5);
		background-color: rgb(230 255 250);
		padding: 10px 20px;
	}
	input::placeholder {
		color: #000000;
	}
	button,
	input {
		border-radius: 50px;
	}

	button {
		margin-top: 15px;
		width: 75%;
		padding: 10px 25px;
		background-color: rgb(199, 241, 190);
	}
</style>
