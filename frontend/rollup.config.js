import svelte from "rollup-plugin-svelte";
import { sveltePreprocess } from "svelte-preprocess";
import postcss from "rollup-plugin-postcss";
import resolve from "@rollup/plugin-node-resolve";
import commonjs from "@rollup/plugin-commonjs";
import typescript from "@rollup/plugin-typescript";
import livereload from "rollup-plugin-livereload";
import { terser } from "rollup-plugin-terser";

const production = !process.env.ROLLUP_WATCH;

export default {
	input: "src/main.ts",
	output: {
		sourcemap: true,
		format: "iife",
		name: "app",
		file: "public/build/bundle.js",
	},
	onwarn(warning, warn) {
		if (warning.code === "CIRCULAR_DEPENDENCY") return; // Ignore circular dependency warnings
		warn(warning); // Default handling for other warnings
	},
	plugins: [
		svelte({
			compilerOptions: {
				dev: !production,
			},
			emitCss: true,
			preprocess: sveltePreprocess({ postcss: true }),
		}),
		postcss({
			extract: true,
			minimize: production,
			sourceMap: !production,
		}),
		resolve({
			browser: true,
			dedupe: ["svelte"],
		}),
		commonjs(),
		typescript({
			sourceMap: !production,
			inlineSources: !production,
		}),
		!production && livereload("public"),
		production && terser(),
	],
	watch: {
		clearScreen: false,
	},
};
