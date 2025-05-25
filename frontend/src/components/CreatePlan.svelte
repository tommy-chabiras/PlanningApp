<script lang="ts">
	import { token, setToken } from "../stores/auth";
	import { displayModal, completeModal } from "../stores/modal";

	let title: string = "";
	let description: string = "";
	let location: string = "";
	let time: string = "";
	let errorStream: { error?: string } = {};
	let error = false;

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
	<form class="login-form" on:submit={createPlan} method="post">
		<input
			class="modal-input"
			type="text"
			name="title"
			bind:value={title}
			placeholder="Plan title"
		/>
		<input
			class="modal-input"
			type="text"
			name="location"
			bind:value={location}
			placeholder="Location"
		/>
		<input
			class="modal-input"
			type="datetime-local"
			on:click={(e) => e.currentTarget.showPicker()}
			name="datetime"
			bind:value={time}
			placeholder="time"
		/>
		<textarea
			class="modal-input"
			name="description"
			bind:value={description}
			placeholder="Enter plan description here."
		>
		</textarea>
		<button class="modal-btn" type="submit">Sign Up</button>
	</form>
</div>
