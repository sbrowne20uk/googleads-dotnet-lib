// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201605;
using Google.Api.Ads.Dfp.v201605;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201605 {
  /// <summary>
  /// This example gets users by email.
  /// </summary>
  public class GetUserByEmailAddress : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets users by email.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetUserByEmailAddress codeExample = new GetUserByEmailAddress();
      Console.WriteLine(codeExample.Description);

      string emailAddress = _T("INSERT_EMAIL_ADDRESS_HERE");
      codeExample.Run(new DfpUser(), emailAddress);
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser user, string emailAddress) {
      UserService userService =
          (UserService) user.GetService(DfpService.v201605.UserService);

      // Create a statement to select users.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("email = :email")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("email", emailAddress);

      // Retrieve a small amount of users at a time, paging through
      // until all users have been retrieved.
      UserPage page = new UserPage();
      try {
        do {
          page = userService.getUsersByStatement(statementBuilder.ToStatement());

          if (page.results != null) {
            // Print out some information for each user.
            int i = page.startIndex;
            foreach (User usr in page.results) {
              Console.WriteLine("{0}) User with ID \"{1}\" and name \"{2}\" was found.",
                  i++,
                  usr.id,
                  usr.name);
            }
          }

          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get users. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
