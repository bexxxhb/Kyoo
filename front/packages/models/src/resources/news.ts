/*
 * Kyoo - A portable and vast media library solution.
 * Copyright (c) Kyoo.
 *
 * See AUTHORS.md and LICENSE file in the project root for full license information.
 *
 * Kyoo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * Kyoo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Kyoo. If not, see <https://www.gnu.org/licenses/>.
 */

import { z } from "zod";
import { MovieP } from "./movie";
import { BaseEpisodeP } from "./episode";
import { ResourceP } from "../traits/resource";

/**
 * The type of item, ether a a movie or an episode.
 */
export enum NewsKind {
	Episode = "Episode",
	Movie = "Movie",
}

export const NewsP = z.union([
	/*
	 * Either an episode
	 */
	BaseEpisodeP.and(
		z.object({
			kind: z.literal(NewsKind.Episode),
			show: ResourceP.extend({
				name: z.string(),
			}),
		}),
	),
	/*
	 * Or a Movie
	 */
	MovieP.and(z.object({ kind: z.literal(NewsKind.Movie) })),
]);

/**
 * A new item added to kyoo.
 */
export type News = z.infer<typeof NewsP>;