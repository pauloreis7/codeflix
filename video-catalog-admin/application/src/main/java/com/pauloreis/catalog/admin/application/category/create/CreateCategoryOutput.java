package com.pauloreis.catalog.admin.application.category.create;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryID;

public record CreateCategoryOutput(
    CategoryID id) {

  public static CreateCategoryOutput from(final CategoryID anId) {
    return new CreateCategoryOutput(anId);
  }

  public static CreateCategoryOutput from(final Category aCategory) {
    return new CreateCategoryOutput(aCategory.getId());
  }
}
