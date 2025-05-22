<script lang="ts">
	import {completeModal} from "../stores/modal";
	import {setToken} from "../stores/auth";
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
			setToken(data.token);
			window.location.href = "/";
		}
		completeModal();
	}
</script>

<div class="modal">
	<h2 class="modal-title">Login</h2>
	{#if error}
		<p class="error-text">{errorStream.error}</p>
	{/if}
	<form class="login-form" on:submit={login} method="post">
		<input class="modal-input" type="text" bind:value={username} placeholder="Username"/>
		<input class="modal-input" type="password" bind:value={password} placeholder="Password"/>
		<button class="modal-btn" type="submit">Log In</button>
	</form>
</div>

<style>

</style>
