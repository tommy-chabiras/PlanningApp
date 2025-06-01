<script lang="ts">
	import { completeModal } from "../stores/modal";
	import { setToken } from "../stores/auth";

	let displayName = $state("");
	let username = $state("");
	let password = $state("");
	let password2 = $state("");
	let email = $state("");
	let error = $state(false);
	let errorStream: { error?: String } = $state({});

	let pwError = $derived(password2 && password !== password2);

	async function signup(e: Event) {
		e.preventDefault();
		if (password.length > 1 && password2.length > 1) {
			if (!pwError) {
				const response = await fetch("/api/user/signup", {
					method: "POST",
					headers: {
						"Content-Type": "application/json",
					},
					body: JSON.stringify({
						name: displayName,
						username,
						email,
						password,
					}),
				});
				if (!response.ok) {
					errorStream = JSON.parse(await response.text());
					error = true;
				} else {
					const data = await response.json();
					setToken(data.token);
					window.location.href = "/";
				}
			}
		}
		completeModal();
	}
</script>

<div class="modal">
	<h2 class="modal-title">Sign up</h2>
	{#if error}
		<p class="error-text">{errorStream}</p>
	{/if}
	<form class="login-form" onsubmit={signup} method="post">
		<input
			class="modal-input"
			type="text"
			bind:value={displayName}
			placeholder="Display Name"
		/>
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
