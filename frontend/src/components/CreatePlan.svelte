<script lang="ts">
	import { token, setToken } from "../stores/auth";
	import { displayModal, completeModal } from "../stores/modal";

	let title: string = $state("");
	let description: string = $state("");
	let location: string = $state("");
	let time: string = $state("");
	let errorStream: { error?: string } = $state({});
	let error = $state(false);

	async function createPlan(e: Event) {
		e.preventDefault();
		if (!$token) {
			await displayModal("signupGuest");
			if (!$token) {
				return;
			}
		}

		const response = await fetch("/api/plan/create", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${localStorage.getItem("token")}`,
			},
			body: JSON.stringify({ title, location, time, description }),
		});
		if (!response.ok) {
			errorStream = JSON.parse(await response.text());
			error = true;
		} else {
			const data = await response.json();
			setToken(data.token);
		}
		// window.location.href = "/";
		completeModal();
	}
</script>

<div class="modal">
	<h2 class="modal-title">New Plan</h2>
	{#if error}
		<p class="error-text">{errorStream}</p>
	{/if}
	<form class="login-form" onsubmit={createPlan} method="post">
		<input
			class="modal-input"
			type="text"
			name="title"
			placeholder="Plan title"
		/>
		<input
			class="modal-input"
			type="text"
			name="location"
			placeholder="Location"
		/>
		<input
			class="modal-input"
			type="datetime-local"
			onclick={(e) => e.currentTarget.showPicker()}
			name="datetime"
			placeholder="time"
		/>
		<textarea
			class="modal-input"
			name="description"
			placeholder="Enter plan description here."
		>
		</textarea>
		<button class="modal-btn" type="submit">Sign Up</button>
	</form>
</div>
