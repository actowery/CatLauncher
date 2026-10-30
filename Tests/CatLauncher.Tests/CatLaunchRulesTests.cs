using Xunit;

namespace CatLauncher.Tests;

public sealed class CatLaunchRulesTests
{
    [Theory][InlineData(-0.01f, false)][InlineData(0f, true)][InlineData(0.0099f, true)][InlineData(0.01f, false)][InlineData(1f, false)]
    public void Survivor_roll_uses_one_percent_threshold(float roll, bool expected) => Assert.Equal(expected, CatLaunchRules.ShouldSpawnSurvivor(roll));

    [Theory][InlineData(0f, 4)][InlineData(0.5f, 6)][InlineData(0.999f, 7)]
    public void Fragment_count_stays_within_visual_budget(float roll, int expected) => Assert.Equal(expected, CatLaunchRules.FragmentCount(roll));

    [Theory][InlineData(0f, 0.35f)][InlineData(1f, 1f)][InlineData(9f, 2.5f)]
    public void Debris_distance_is_kept_inside_blast_visual_radius(float input, float expected) => Assert.Equal(expected, CatLaunchRules.ClampDebrisDistance(input));
}
