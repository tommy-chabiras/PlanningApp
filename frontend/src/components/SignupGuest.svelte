<script lang="ts">
	import { completeModal } from "../stores/modal";
	import { setToken } from "../stores/auth";

	let name = $state("");
	let error = $state(false);
	let errorStream: { error?: String } = $state({});

	async function registerGuest(e: Event) {
		e.preventDefault();
		const response = await fetch("/api/user/guest", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ name }),
		});

		if (!response.ok) {
			errorStream = JSON.parse(await response.text());
			error = true;
		} else {
			const data = await response.json();
			setToken(data.token);
		}
		completeModal();
	}
</script>

<div class="modal">
	<h2 class="modal-title">Enter a Display Name</h2>
	{#if error}
		<p class="error-text">{errorStream.error}</p>
	{/if}
	<form class="login-form" onsubmit={registerGuest} method="post">
		<input
			class="modal-input"
			type="text"
			bind:value={name}
			placeholder="Display name"
		/>
		<button class="modal-btn" type="submit">Log In</button>
	</form>
</div>
