package com.pauloreis.catalog.admin.application.category.update;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryID;

public record UpdateCategoryOutput(CategoryID id) {

  public static UpdateCategoryOutput from(final Category aCategory) {
    return new UpdateCategoryOutput(aCategory.getId());
  }
}
