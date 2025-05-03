<script lang="ts">
	let username = "";
	let password = "";
	let password2 = "";
	let email = "";
	let error = false;
	let errorStream: { error?: String };

	$: pwError = password2 && password !== password2;

	async function login(e: Event) {
		e.preventDefault();
		if (password.length > 1 && password2.length > 1) {
			if (password === password2) {
				pwError = false;
				const response = await fetch("/api/user/signup", {
					method: "POST",
					headers: {
						"Content-Type": "application/json",
					},
					body: JSON.stringify({ username, email, password }),
				});

				if (!response.ok) {
					errorStream = JSON.parse(await response.text());
					error = true;
				} else {
					const data = await response.json();
					localStorage.setItem("token", data.token);
					window.location.href = "/";
				}
			} else {
				pwError = true;
			}
		}
	}
</script>

<div class="modal-con">
	<h2 class="modal-title">Sign up</h2>
	{#if error}
		<p class="error-text">{errorStream.error}</p>
	{/if}
	<form class="login-form" on:submit={login} method="post">
		<input
			class="modal-input"
			type="text"
			bind:value={username}
			placeholder="Username"
		/>
		<input
			class="modal-input"
			type="email"
			bind:value={email}
			placeholder="Email"
		/>
		{#if pwError}
			<p class="error-text">Passwords don't match</p>
		{/if}
		<input
			class="modal-input"
			type="password"
			bind:value={password}
			placeholder="Password"
		/>
		<input
			class="modal-input"
			type="password"
			bind:value={password2}
			placeholder="Confirm Password"
		/>
		<button class="modal-btn" type="submit">Sign Up</button>
	</form>
</div>

<style>
</style>
