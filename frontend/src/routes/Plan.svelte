<script lang="ts">
	import { onMount } from "svelte";
	let { params } = $props();

	let plan: any = $state(null);
	let users: any[] = $state([]);

	onMount(async () => {
		// GET PLAN
		let response = await fetch(`/api/plan/${params.plancode}`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("token")}`,
			},
		});
		if (response.ok) {
			plan = await response.json();
		}

		// GET PLAN PARTICIPANTS
		let response1 = await fetch(`/api/plan/get-users`, {
			method: "POST",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("token")}`,
				"Content-Type": "application/json",
			},
			body: JSON.stringify(params.plancode),
		});
		if (response1.ok) {
			users = await response1.json();
			// console.log(users);
		}
	});

	// EDIT PLAN
	function editPlan() {
		
	}
	
	const response = fetch(`/api/plan/${params.plancode}`, {
		method: "PUT",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("token")}`,
			"Content-type": "application/json",
		},
		// body: JSON.stringify()
	});


	const admin = true;
	let editing = $state(false);
</script>

{#if plan}
	<div class="plan-con">
		<div class="plan-header">
			{#if admin}
				<button class="plan-settings" onclick="{() => editing = editing ? true : false}">
					<span class="material-icons"> settings </span>
				</button>
			{/if}
			<h1>{plan.title}</h1>
		</div>
		<div class="participants-con">
			<strong>Participants:</strong>
			{#each users as user, i}
				<div class="participant">
					<p class="participant-text">{user.name}</p>
				</div>
			{/each}
		</div>
		{#if editing}
			<!-- <p>test</p> -->
			<div class="info-con">
				<strong>Time:</strong>
				<p>{plan.time}</p>
				<strong>Location:</strong>
				<p>{plan.location}</p>
				{#if plan.description}
					<strong>Description:</strong>
					<p>{plan.description}</p>
				{/if}
			</div>
		{:else}
			<div class="info-con">
				<strong>Time:</strong>
				<p>{plan.time}</p>
				<strong>Location:</strong>
				<p>{plan.location}</p>
				{#if plan.description}
					<strong>Description:</strong>
					<p>{plan.description}</p>
				{/if}
			</div>
		{/if}
		<div class="comments-con">
			<strong>Discussion:</strong>
			<form class="comment-form" id="comment-form">
				<textarea
					class="comment-field"
					name="comment"
					placeholder="Add a comment"
				></textarea>
				<button class="comment-button">Add comment</button>
			</form>
			<ul>
				<!-- {#each comment as comments, i}

				{/each} -->
			</ul>
		</div>
	</div>
{/if}
