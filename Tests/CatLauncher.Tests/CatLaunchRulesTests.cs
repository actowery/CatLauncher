using Xunit;

namespace CatLauncher.Tests;

public sealed class CatLaunchRulesTests
{
    [Theory][InlineData(-0.01f, false)][InlineData(0f, true)][InlineData(0.0099f, true)][InlineData(0.01f, false)][InlineData(1f, false)]
    public void Survivor_roll_uses_one_percent_threshold(float roll, bool expected) => Assert.Equal(expected, CatLaunchRules.ShouldSpawnSurvivor(roll));

    [Theory][InlineData(-0.01f, 0.5f, false)][InlineData(0.01f, 0.02f, true)][InlineData(0.02f, 0.02f, false)][InlineData(0f, -1f, false)][InlineData(0.9f, 0f, false)][InlineData(0.99f, 5f, true)][InlineData(1f, 5f, false)]
    public void Survivor_roll_respects_configured_chance(float roll, float chance, bool expected) => Assert.Equal(expected, CatLaunchRules.ShouldSpawnSurvivor(roll, chance));

    [Theory][InlineData(0f, 4)][InlineData(0.5f, 6)][InlineData(0.999f, 7)]
    public void Fragment_count_stays_within_visual_budget(float roll, int expected) => Assert.Equal(expected, CatLaunchRules.FragmentCount(roll));

    [Theory][InlineData(-2, 1f, 0)][InlineData(6, -1f, 0)][InlineData(6, 0f, 0)][InlineData(6, 0.5f, 3)][InlineData(6, 1f, 6)][InlineData(7, 3f, 21)][InlineData(7, 4f, 21)]
    public void Fragment_count_scales_with_bloodiness_but_stays_capped(int count, float multiplier, int expected) => Assert.Equal(expected, CatLaunchRules.ScaleFragmentCount(count, multiplier));

    [Theory][InlineData(0f, 0.35f)][InlineData(1f, 1f)][InlineData(9f, 2.5f)]
    public void Debris_distance_is_kept_inside_blast_visual_radius(float input, float expected) => Assert.Equal(expected, CatLaunchRules.ClampDebrisDistance(input));

    [Theory][InlineData(0, 0)][InlineData(3, 3)][InlineData(4, 0)][InlineData(-1, 3)]
    public void Cat_facing_uses_a_valid_direction(int input, int expected) => Assert.Equal(expected, CatLaunchRules.NormalizeFacing(input));

    [Theory][InlineData(-1, 4, 0)][InlineData(0, 4, 0)][InlineData(3, 4, 3)][InlineData(4, 4, 3)][InlineData(9, 4, 3)][InlineData(4, 0, 0)]
    public void Survivor_tint_index_is_always_a_valid_defined_coat(int input, int colorCount, int expected) => Assert.Equal(expected, CatLaunchRules.NormalizeColorIndex(input, colorCount));

    [Theory][InlineData(0, 1, 12, 30f)][InlineData(2, -1, 12, 150f)]
    public void Cat_spin_is_independent_from_its_facing(int facing, int spinDirection, int ticks, float expected) => Assert.Equal(expected, CatLaunchRules.FlightRotation(facing, spinDirection, ticks));
}
