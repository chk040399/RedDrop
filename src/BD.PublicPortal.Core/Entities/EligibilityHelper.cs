using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BD.PublicPortal.Core.Entities;

/*
O-: universal donor(can donate to all)

AB+: universal recipient(can receive from all)

Negative(-) blood types can donate to both - and + of same letter types

Positive(+) blood types can only donate to + types

*/


public static class EligibilityHelper
{
  public static IEnumerable<BloodGroup> ReceiverGroupToDonnorsGroups(BloodGroup receiverGroup)
  {
    return receiverGroup switch
    {
      BloodGroup.AB_POSITIVE => AllBloodGroups(),
      BloodGroup.AB_NEGATIVE => new[] { BloodGroup.AB_NEGATIVE, BloodGroup.A_NEGATIVE, BloodGroup.B_NEGATIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.A_POSITIVE => new[] { BloodGroup.A_POSITIVE, BloodGroup.A_NEGATIVE, BloodGroup.O_POSITIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.A_NEGATIVE => new[] { BloodGroup.A_NEGATIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.B_POSITIVE => new[] { BloodGroup.B_POSITIVE, BloodGroup.B_NEGATIVE, BloodGroup.O_POSITIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.B_NEGATIVE => new[] { BloodGroup.B_NEGATIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.O_POSITIVE => new[] { BloodGroup.O_POSITIVE, BloodGroup.O_NEGATIVE },
      BloodGroup.O_NEGATIVE => new[] { BloodGroup.O_NEGATIVE },
      _ => Enumerable.Empty<BloodGroup>()
    };
  }

  public static IEnumerable<BloodGroup> DonnorGroupToReceiverGroups(BloodGroup donnorGroup)
  {
    return donnorGroup switch
    {
      BloodGroup.O_NEGATIVE => AllBloodGroups(),
      BloodGroup.O_POSITIVE => new[] { BloodGroup.O_POSITIVE, BloodGroup.A_POSITIVE, BloodGroup.B_POSITIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.A_NEGATIVE => new[] { BloodGroup.A_NEGATIVE, BloodGroup.A_POSITIVE, BloodGroup.AB_NEGATIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.A_POSITIVE => new[] { BloodGroup.A_POSITIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.B_NEGATIVE => new[] { BloodGroup.B_NEGATIVE, BloodGroup.B_POSITIVE, BloodGroup.AB_NEGATIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.B_POSITIVE => new[] { BloodGroup.B_POSITIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.AB_NEGATIVE => new[] { BloodGroup.AB_NEGATIVE, BloodGroup.AB_POSITIVE },
      BloodGroup.AB_POSITIVE => new[] { BloodGroup.AB_POSITIVE },
      _ => Enumerable.Empty<BloodGroup>()
    };
  }

  public static bool CanDonateTo(BloodGroup donorGroup, BloodGroup receiverGroup)
  {
    return DonnorGroupToReceiverGroups(donorGroup).Contains(receiverGroup);
  }

  public static string GenerateCompatibilityHtmlTable()
  {
    var groups = Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>().ToList();
    var sb = new System.Text.StringBuilder();

    sb.AppendLine("<table border='1' cellspacing='0' cellpadding='5'>");
    sb.AppendLine("<thead><tr><th>Donor \\ Recipient</th>");

    foreach (var recipient in groups)
      sb.AppendLine($"<th>{FormatBloodGroup(recipient)}</th>");
    sb.AppendLine("</tr></thead><tbody>");

    foreach (var donor in groups)
    {
      sb.AppendLine($"<tr><td><strong>{FormatBloodGroup(donor)}</strong></td>");
      foreach (var recipient in groups)
      {
        var compatible = CanDonateTo(donor, recipient);
        var cell = compatible ? "✅" : "❌";
        sb.AppendLine($"<td style='text-align:center'>{cell}</td>");
      }
      sb.AppendLine("</tr>");
    }

    sb.AppendLine("</tbody></table>");
    return sb.ToString();
  }

  public static string GenerateInvertedCompatibilityHtmlTable()
  {
    var orderedGroups = new List<BloodGroup>
    {
        BloodGroup.O_NEGATIVE,
        BloodGroup.O_POSITIVE,
        BloodGroup.B_NEGATIVE,
        BloodGroup.B_POSITIVE,
        BloodGroup.A_NEGATIVE,
        BloodGroup.A_POSITIVE,
        BloodGroup.AB_NEGATIVE,
        BloodGroup.AB_POSITIVE
    };

    var reversedGroups = orderedGroups.AsEnumerable().Reverse().ToList();

    var sb = new System.Text.StringBuilder();

    sb.AppendLine("<table border='1' cellspacing='0' cellpadding='5'>");
    sb.AppendLine("<thead><tr><th>Recipient \\ Donor</th>");

    foreach (var donor in orderedGroups)
      sb.AppendLine($"<th>{FormatBloodGroup(donor)}</th>");
    sb.AppendLine("</tr></thead><tbody>");

    foreach (var recipient in reversedGroups)
    {
      sb.AppendLine($"<tr><td><strong>{FormatBloodGroup(recipient)}</strong></td>");
      foreach (var donor in orderedGroups)
      {
        var compatible = CanDonateTo(donor, recipient);
        var cell = compatible ? "✅" : "❌";
        sb.AppendLine($"<td style='text-align:center'>{cell}</td>");
      }
      sb.AppendLine("</tr>");
    }

    sb.AppendLine("</tbody></table>");
    return sb.ToString();
  }

  private static string FormatBloodGroup(BloodGroup group)
  {
    return group switch
    {
      BloodGroup.A_POSITIVE => "A+",
      BloodGroup.A_NEGATIVE => "A−",
      BloodGroup.B_POSITIVE => "B+",
      BloodGroup.B_NEGATIVE => "B−",
      BloodGroup.AB_POSITIVE => "AB+",
      BloodGroup.AB_NEGATIVE => "AB−",
      BloodGroup.O_POSITIVE => "O+",
      BloodGroup.O_NEGATIVE => "O−",
      _ => group.ToString()
    };
  }

  private static IEnumerable<BloodGroup> AllBloodGroups()
  {
    return Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>();
  }
}
